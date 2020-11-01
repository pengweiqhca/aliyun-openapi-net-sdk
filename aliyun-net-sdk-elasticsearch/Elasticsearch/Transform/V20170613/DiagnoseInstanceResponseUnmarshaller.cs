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
using System;
using System.Collections.Generic;

using Aliyun.Acs.Core.Transform;
using Aliyun.Acs.elasticsearch.Model.V20170613;

namespace Aliyun.Acs.elasticsearch.Transform.V20170613
{
    public class DiagnoseInstanceResponseUnmarshaller
    {
        public static DiagnoseInstanceResponse Unmarshall(UnmarshallerContext context)
        {
			DiagnoseInstanceResponse diagnoseInstanceResponse = new DiagnoseInstanceResponse();

			diagnoseInstanceResponse.HttpResponse = context.HttpResponse;
			diagnoseInstanceResponse.RequestId = context.StringValue("DiagnoseInstance.RequestId");

			DiagnoseInstanceResponse.DiagnoseInstance_Result result = new DiagnoseInstanceResponse.DiagnoseInstance_Result();
			result.ReportId = context.StringValue("DiagnoseInstance.Result.reportId");
			result.InstanceId = context.StringValue("DiagnoseInstance.Result.instanceId");
			result.State = context.StringValue("DiagnoseInstance.Result.state");
			result.CreateTime = context.LongValue("DiagnoseInstance.Result.createTime");

			List<DiagnoseInstanceResponse.DiagnoseInstance_Result.DiagnoseInstance_DiagnoseItemsItem> result_diagnoseItems = new List<DiagnoseInstanceResponse.DiagnoseInstance_Result.DiagnoseInstance_DiagnoseItemsItem>();
			for (int i = 0; i < context.Length("DiagnoseInstance.Result.DiagnoseItems.Length"); i++) {
				DiagnoseInstanceResponse.DiagnoseInstance_Result.DiagnoseInstance_DiagnoseItemsItem diagnoseItemsItem = new DiagnoseInstanceResponse.DiagnoseInstance_Result.DiagnoseInstance_DiagnoseItemsItem();
				diagnoseItemsItem.Item = context.StringValue("DiagnoseInstance.Result.DiagnoseItems["+ i +"].item");
				diagnoseItemsItem.Health = context.StringValue("DiagnoseInstance.Result.DiagnoseItems["+ i +"].health");

				DiagnoseInstanceResponse.DiagnoseInstance_Result.DiagnoseInstance_DiagnoseItemsItem.DiagnoseInstance_Detail detail = new DiagnoseInstanceResponse.DiagnoseInstance_Result.DiagnoseInstance_DiagnoseItemsItem.DiagnoseInstance_Detail();
				detail.Name = context.StringValue("DiagnoseInstance.Result.DiagnoseItems["+ i +"].Detail.name");
				detail.Desc = context.StringValue("DiagnoseInstance.Result.DiagnoseItems["+ i +"].Detail.desc");
				detail.Type = context.StringValue("DiagnoseInstance.Result.DiagnoseItems["+ i +"].Detail.type");
				detail.Suggest = context.StringValue("DiagnoseInstance.Result.DiagnoseItems["+ i +"].Detail.suggest");
				detail.Result = context.StringValue("DiagnoseInstance.Result.DiagnoseItems["+ i +"].Detail.result");
				diagnoseItemsItem.Detail = detail;

				result_diagnoseItems.Add(diagnoseItemsItem);
			}
			result.DiagnoseItems = result_diagnoseItems;
			diagnoseInstanceResponse.Result = result;
        
			return diagnoseInstanceResponse;
        }
    }
}
