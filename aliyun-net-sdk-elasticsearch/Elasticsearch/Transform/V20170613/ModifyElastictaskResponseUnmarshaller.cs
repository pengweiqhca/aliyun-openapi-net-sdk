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
    public class ModifyElastictaskResponseUnmarshaller
    {
        public static ModifyElastictaskResponse Unmarshall(UnmarshallerContext context)
        {
			ModifyElastictaskResponse modifyElastictaskResponse = new ModifyElastictaskResponse();

			modifyElastictaskResponse.HttpResponse = context.HttpResponse;
			modifyElastictaskResponse.RequestId = context.StringValue("ModifyElastictask.RequestId");

			ModifyElastictaskResponse.ModifyElastictask_Result result = new ModifyElastictaskResponse.ModifyElastictask_Result();

			ModifyElastictaskResponse.ModifyElastictask_Result.ModifyElastictask_ElasticExpansionTask elasticExpansionTask = new ModifyElastictaskResponse.ModifyElastictask_Result.ModifyElastictask_ElasticExpansionTask();
			elasticExpansionTask.TriggerType = context.StringValue("ModifyElastictask.Result.ElasticExpansionTask.triggerType");
			elasticExpansionTask.CronExpression = context.StringValue("ModifyElastictask.Result.ElasticExpansionTask.cronExpression");
			elasticExpansionTask.ElasticNodeCount = context.IntegerValue("ModifyElastictask.Result.ElasticExpansionTask.elasticNodeCount");
			elasticExpansionTask.ReplicaCount = context.IntegerValue("ModifyElastictask.Result.ElasticExpansionTask.replicaCount");

			List<string> elasticExpansionTask_targetIndices = new List<string>();
			for (int i = 0; i < context.Length("ModifyElastictask.Result.ElasticExpansionTask.TargetIndices.Length"); i++) {
				elasticExpansionTask_targetIndices.Add(context.StringValue("ModifyElastictask.Result.ElasticExpansionTask.TargetIndices["+ i +"]"));
			}
			elasticExpansionTask.TargetIndices = elasticExpansionTask_targetIndices;
			result.ElasticExpansionTask = elasticExpansionTask;

			ModifyElastictaskResponse.ModifyElastictask_Result.ModifyElastictask_ElasticShrinkTask elasticShrinkTask = new ModifyElastictaskResponse.ModifyElastictask_Result.ModifyElastictask_ElasticShrinkTask();
			elasticShrinkTask.TriggerType = context.StringValue("ModifyElastictask.Result.ElasticShrinkTask.triggerType");
			elasticShrinkTask.CronExpression = context.StringValue("ModifyElastictask.Result.ElasticShrinkTask.cronExpression");
			elasticShrinkTask.ElasticNodeCount = context.IntegerValue("ModifyElastictask.Result.ElasticShrinkTask.elasticNodeCount");
			elasticShrinkTask.ReplicaCount = context.IntegerValue("ModifyElastictask.Result.ElasticShrinkTask.replicaCount");

			List<string> elasticShrinkTask_targetIndices1 = new List<string>();
			for (int i = 0; i < context.Length("ModifyElastictask.Result.ElasticShrinkTask.TargetIndices.Length"); i++) {
				elasticShrinkTask_targetIndices1.Add(context.StringValue("ModifyElastictask.Result.ElasticShrinkTask.TargetIndices["+ i +"]"));
			}
			elasticShrinkTask.TargetIndices1 = elasticShrinkTask_targetIndices1;
			result.ElasticShrinkTask = elasticShrinkTask;
			modifyElastictaskResponse.Result = result;
        
			return modifyElastictaskResponse;
        }
    }
}
