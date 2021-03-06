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

namespace Aliyun.Acs.facebody.Model.V20191230
{
	public class DetectMaskResponse : AcsResponse
	{

		private string requestId;

		private DetectMask_Data data;

		public string RequestId
		{
			get
			{
				return requestId;
			}
			set	
			{
				requestId = value;
			}
		}

		public DetectMask_Data Data
		{
			get
			{
				return data;
			}
			set	
			{
				data = value;
			}
		}

		public class DetectMask_Data
		{

			private int? mask;

			private float? faceProbability;

			public int? Mask
			{
				get
				{
					return mask;
				}
				set	
				{
					mask = value;
				}
			}

			public float? FaceProbability
			{
				get
				{
					return faceProbability;
				}
				set	
				{
					faceProbability = value;
				}
			}
		}
	}
}
