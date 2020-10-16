﻿/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core.Utils;

namespace Aliyun.Acs.Core.Auth.Provider
{
    public partial class DefaultCredentialProvider
    {
        public async Task<AlibabaCloudCredentials> GetAlibabaCloudClientCredentialAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var credential = await GetEnvironmentAlibabaCloudCredentialAsync(cancellationToken).ConfigureAwait(false) ??
                             await GetCredentialFileAlibabaCloudCredentialAsync(cancellationToken).ConfigureAwait(false) ??
                             await GetInstanceRamRoleAlibabaCloudCredentialAsync(cancellationToken).ConfigureAwait(false);

            if (credential == null)
            {
                throw new ClientException("There is no credential chain can use.");
            }

            return credential;
        }

        public async Task<AlibabaCloudCredentials> GetEnvironmentAlibabaCloudCredentialAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (null == accessKeyId || null == accessKeySecret)
            {
                return null;
            }

            if (accessKeyId.Equals("") || accessKeySecret.Equals(""))
            {
                throw new ClientException(
                    "Environment credential variable 'ALIBABA_CLOUD_ACCESS_KEY_*' cannot be empty");
            }

            return defaultProfile.DefaultClientName.Equals("default") ? await GetAccessKeyCredentialAsync(cancellationToken).ConfigureAwait(false) : null;
        }

        public async Task<AlibabaCloudCredentials> GetCredentialFileAlibabaCloudCredentialAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (null == credentialFileLocation)
            {
                credentialFileLocation = GetHomePath();
                var slash = EnvironmentUtil.GetOSSlash();
                var fileLocation = EnvironmentUtil.GetComposedPath(credentialFileLocation, slash);

                if (File.Exists(fileLocation))
                {
                    credentialFileLocation = fileLocation;
                }
                else
                {
                    return null;
                }
            }

            if (credentialFileLocation.Equals(""))
            {
                throw new ClientException(
                    "Credentials file environment variable 'ALIBABA_CLOUD_CREDENTIALS_FILE' cannot be empty");
            }

            var iniReader = new IniReader(credentialFileLocation);
            var sectionNameList = iniReader.GetSections();

            if (null != defaultProfile.DefaultClientName)
            {
                var userDefineSectionNode = defaultProfile.DefaultClientName;

                var iniKeyTypeValue = iniReader.GetValue("type", userDefineSectionNode);

                if (iniKeyTypeValue.Equals("access_key"))
                {
                    accessKeyId = iniReader.GetValue("access_key_id", userDefineSectionNode);
                    accessKeySecret = iniReader.GetValue("access_key_secret", userDefineSectionNode);
                    regionId = iniReader.GetValue("region_id", userDefineSectionNode);

                    return await GetAccessKeyCredentialAsync(cancellationToken).ConfigureAwait(false);
                }

                if (iniKeyTypeValue.Equals("ecs_ram_role"))
                {
                    roleName = iniReader.GetValue("role_name", userDefineSectionNode);
                    regionId = iniReader.GetValue("region_id", userDefineSectionNode);

                    return await GetInstanceRamRoleAlibabaCloudCredentialAsync(cancellationToken).ConfigureAwait(false);
                }

                if (iniKeyTypeValue.Equals("ram_role_arn"))
                {
                    accessKeyId = iniReader.GetValue("access_key_id", userDefineSectionNode);
                    accessKeySecret = iniReader.GetValue("access_key_secret", userDefineSectionNode);
                    roleArn = iniReader.GetValue("role_arn", userDefineSectionNode);

                    return await GetRamRoleArnAlibabaCloudCredentialAsync(cancellationToken).ConfigureAwait(false);
                }

                if (iniKeyTypeValue.Equals("rsa_key_pair"))
                {
                    publicKeyId = iniReader.GetValue("public_key_id", userDefineSectionNode);
                    privateKeyFile = iniReader.GetValue("private_key_file", userDefineSectionNode);

                    return await GetRsaKeyPairAlibabaCloudCredentialAsync(cancellationToken).ConfigureAwait(false);
                }
            }
            else
            {
                foreach (var sectionItem in sectionNameList)
                {
                    if (!sectionItem.Equals("default"))
                    {
                        continue;
                    }

                    accessKeyId = iniReader.GetValue("access_key_id", "default");
                    accessKeySecret = iniReader.GetValue("access_key_secret", "default");
                    regionId = iniReader.GetValue("region_id", "default");

                    return await GetAccessKeyCredentialAsync().ConfigureAwait(false);
                }
            }

            return null;
        }

        public virtual Task<AlibabaCloudCredentials> GetInstanceRamRoleAlibabaCloudCredentialAsync(CancellationToken cancellationToken)
        {
            if (null == regionId || regionId.Equals(""))
            {
                throw new ClientException("RegionID cannot be null or empty.");
            }

            if (!defaultProfile.DefaultClientName.Equals("default"))
            {
                return null;
            }

            InstanceProfileCredentialsProvider instanceProfileCredentialProvider;
            if (null != alibabaCloudCredentialProvider)
            {
                instanceProfileCredentialProvider =
                    (InstanceProfileCredentialsProvider)alibabaCloudCredentialProvider;
            }
            else
            {
                instanceProfileCredentialProvider = new InstanceProfileCredentialsProvider(roleName);
            }

            return instanceProfileCredentialProvider.GetCredentialsAsync(cancellationToken);
        }

        public Task<AlibabaCloudCredentials> GetAccessKeyCredentialAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(accessKeyId) || string.IsNullOrEmpty(accessKeySecret) ||
                string.IsNullOrEmpty(regionId))
            {
                throw new ClientException("Missing required variable option for 'default Client'");
            }

            var accessKeyCredentialProvider =
                new AccessKeyCredentialProvider(accessKeyId, accessKeySecret);

            return accessKeyCredentialProvider.GetCredentialsAsync(cancellationToken);
        }

        public virtual Task<AlibabaCloudCredentials> GetRamRoleArnAlibabaCloudCredentialAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(accessKeyId) || string.IsNullOrEmpty(accessKeySecret) ||
                string.IsNullOrEmpty(regionId))
            {
                throw new ClientException("Missing required variable option for 'default Client'");
            }

            var credential = new BasicSessionCredentials(accessKeyId, accessKeySecret,
                STSAssumeRoleSessionCredentialsProvider.GetNewRoleSessionName(),
                3600
            );
            var profile = DefaultProfile.GetProfile(regionId, accessKeyId, accessKeySecret);

            STSAssumeRoleSessionCredentialsProvider stsAsssumeRoleSessionCredentialProvider;

            if (null != alibabaCloudCredentialProvider)
            {
                stsAsssumeRoleSessionCredentialProvider =
                    (STSAssumeRoleSessionCredentialsProvider)alibabaCloudCredentialProvider;
            }
            else
            {
                stsAsssumeRoleSessionCredentialProvider =
                    new STSAssumeRoleSessionCredentialsProvider(credential, roleArn, profile);
            }

            return stsAsssumeRoleSessionCredentialProvider.GetCredentialsAsync(cancellationToken);
        }

        public virtual Task<AlibabaCloudCredentials> GetRsaKeyPairAlibabaCloudCredentialAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(publicKeyId) || string.IsNullOrEmpty(privateKeyFile) ||
                string.IsNullOrEmpty(regionId))
            {
                throw new ClientException("Missing required variable option for 'default Client'");
            }

            var rsaKeyPairCredential = new KeyPairCredentials(publicKeyId, privateKeyFile);
            var profile = DefaultProfile.GetProfile(regionId, publicKeyId, privateKeyFile);

            RsaKeyPairCredentialProvider rsaKeyPairCredentialProvider;

            if (null != alibabaCloudCredentialProvider)
            {
                rsaKeyPairCredentialProvider = (RsaKeyPairCredentialProvider)alibabaCloudCredentialProvider;
            }
            else
            {
                rsaKeyPairCredentialProvider = new RsaKeyPairCredentialProvider(rsaKeyPairCredential, profile);
            }

            return rsaKeyPairCredentialProvider.GetCredentialsAsync(cancellationToken);
        }
    }
}
