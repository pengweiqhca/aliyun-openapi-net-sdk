/*
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
using System.Collections.Generic;

using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Transform;
using Aliyun.Acs.Core.Utils;
using Aliyun.Acs.tdsr;
using Aliyun.Acs.tdsr.Transform;
using Aliyun.Acs.tdsr.Transform.V20200101;

namespace Aliyun.Acs.tdsr.Model.V20200101
{
    public class CreateProjectRequest : RpcAcsRequest<CreateProjectResponse>
    {
        public CreateProjectRequest()
            : base("tdsr", "2020-01-01", "CreateProject")
        {
            if (this.GetType().GetProperty("ProductEndpointMap") != null && this.GetType().GetProperty("ProductEndpointType") != null)
            {
                this.GetType().GetProperty("ProductEndpointMap").SetValue(this, Aliyun.Acs.tdsr.Endpoint.endpointMap, null);
                this.GetType().GetProperty("ProductEndpointType").SetValue(this, Aliyun.Acs.tdsr.Endpoint.endpointRegionalType, null);
            }
			Method = MethodType.POST;
        }

		private string businessUserIdList;

		private string builderUserIdList;

		private string name;

		private string businessId;

		private string gatherUserIdList;

		public string BusinessUserIdList
		{
			get
			{
				return businessUserIdList;
			}
			set	
			{
				businessUserIdList = value;
				DictionaryUtil.Add(QueryParameters, "BusinessUserIdList", value);
			}
		}

		public string BuilderUserIdList
		{
			get
			{
				return builderUserIdList;
			}
			set	
			{
				builderUserIdList = value;
				DictionaryUtil.Add(QueryParameters, "BuilderUserIdList", value);
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
			set	
			{
				name = value;
				DictionaryUtil.Add(QueryParameters, "Name", value);
			}
		}

		public string BusinessId
		{
			get
			{
				return businessId;
			}
			set	
			{
				businessId = value;
				DictionaryUtil.Add(QueryParameters, "BusinessId", value);
			}
		}

		public string GatherUserIdList
		{
			get
			{
				return gatherUserIdList;
			}
			set	
			{
				gatherUserIdList = value;
				DictionaryUtil.Add(QueryParameters, "GatherUserIdList", value);
			}
		}

		public override bool CheckShowJsonItemName()
		{
			return false;
		}

        public override CreateProjectResponse GetResponse(UnmarshallerContext unmarshallerContext)
        {
            return CreateProjectResponseUnmarshaller.Unmarshall(unmarshallerContext);
        }
    }
}
