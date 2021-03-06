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
using Aliyun.Acs.digitalstore.Transform;
using Aliyun.Acs.digitalstore.Transform.V20200107;

namespace Aliyun.Acs.digitalstore.Model.V20200107
{
    public class UpdateWarehouseRequest : RpcAcsRequest<UpdateWarehouseResponse>
    {
        public UpdateWarehouseRequest()
            : base("digitalstore", "2020-01-07", "UpdateWarehouse", "digitalstore", "openAPI")
        {
            if (this.GetType().GetProperty("ProductEndpointMap") != null && this.GetType().GetProperty("ProductEndpointType") != null)
            {
                this.GetType().GetProperty("ProductEndpointMap").SetValue(this, Endpoint.endpointMap, null);
                this.GetType().GetProperty("ProductEndpointType").SetValue(this, Endpoint.endpointRegionalType, null);
            }
        }

		private string phoneNumber;

		private string description;

		private string warehouseCategoryCode;

		private string businessModelCode;

		private string warehouseId;

		private string contact;

		private string address;

		private int? disable;

		private string name;

		public string PhoneNumber
		{
			get
			{
				return phoneNumber;
			}
			set	
			{
				phoneNumber = value;
				DictionaryUtil.Add(BodyParameters, "PhoneNumber", value);
			}
		}

		public string Description
		{
			get
			{
				return description;
			}
			set	
			{
				description = value;
				DictionaryUtil.Add(BodyParameters, "Description", value);
			}
		}

		public string WarehouseCategoryCode
		{
			get
			{
				return warehouseCategoryCode;
			}
			set	
			{
				warehouseCategoryCode = value;
				DictionaryUtil.Add(BodyParameters, "WarehouseCategoryCode", value);
			}
		}

		public string BusinessModelCode
		{
			get
			{
				return businessModelCode;
			}
			set	
			{
				businessModelCode = value;
				DictionaryUtil.Add(BodyParameters, "BusinessModelCode", value);
			}
		}

		public string WarehouseId
		{
			get
			{
				return warehouseId;
			}
			set	
			{
				warehouseId = value;
				DictionaryUtil.Add(BodyParameters, "WarehouseId", value);
			}
		}

		public string Contact
		{
			get
			{
				return contact;
			}
			set	
			{
				contact = value;
				DictionaryUtil.Add(BodyParameters, "Contact", value);
			}
		}

		public string Address
		{
			get
			{
				return address;
			}
			set	
			{
				address = value;
				DictionaryUtil.Add(BodyParameters, "Address", value);
			}
		}

		public int? Disable
		{
			get
			{
				return disable;
			}
			set	
			{
				disable = value;
				DictionaryUtil.Add(BodyParameters, "Disable", value.ToString());
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
				DictionaryUtil.Add(BodyParameters, "Name", value);
			}
		}

        public override UpdateWarehouseResponse GetResponse(UnmarshallerContext unmarshallerContext)
        {
            return UpdateWarehouseResponseUnmarshaller.Unmarshall(unmarshallerContext);
        }
    }
}
