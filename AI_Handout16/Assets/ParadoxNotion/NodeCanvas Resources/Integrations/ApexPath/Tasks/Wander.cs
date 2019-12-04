using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Apex;
using Apex.Messages;
using Apex.Services;
using Apex.Steering;
using Apex.Units;
using Apex.WorldGeometry;
using Apex.Steering.Behaviours;

namespace NodeCanvas.Tasks.ApexPath{

	public class Wander : ApexBaseActionTask {

		public BBParameter<float> minWanderDistance = 5;
		public BBParameter<float> maxWanderDistance = 20;
		public BBParameter<float> lingerTime = 0;
		public bool repeat = true;

		protected override void OnExecute(){
			unit.Wander(maxWanderDistance.value - minWanderDistance.value, minWanderDistance.value, lingerTime.value);
		}

		protected override void OnUpdate(){
			if (!repeat){
				var e = unit.lastNavigationEvent;
				if (e == UnitNavigationEventMessage.Event.None){
					return;
				}

				EndAction( e == UnitNavigationEventMessage.Event.DestinationReached );
			}
		}

		protected override void OnStop(){
			unit.StopWander();
		}
	}
}