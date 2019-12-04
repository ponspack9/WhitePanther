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
using Apex.Steering.Components;

namespace NodeCanvas.Tasks.ApexPath{

	public class Flee : ApexBaseActionTask {

		[RequiredField]
		public BBParameter<GameObject> target;
		public BBParameter<float> fledDistance = 10f;
		public BBParameter<float> lookAhead = 2f;

		private Vector3? lastTargetPos;

		protected override string info{
			get {return string.Format("Flee from {0}", target);}
		}

		protected override void OnExecute(){
			var targetPos = target.value.transform.position;
			if ( (unit.position - targetPos).magnitude >= fledDistance.value ){
				EndAction(true);
				return;
			}			
		}

		protected override void OnUpdate(){
			var targetPos = target.value.transform.position;
			var distance = fledDistance.value;
			if ( (unit.position - targetPos).magnitude >= distance ){
				EndAction(true);
				return;
			}

			if (lastTargetPos != targetPos){
				lastTargetPos = targetPos;
				var fleePos = targetPos + ( unit.position - targetPos ).normalized * (distance + lookAhead.value);
				var grid = GridManager.instance.GetGrid(unit.position);
				var cell = grid.GetNearestWalkableCell(fleePos, fleePos, true, Mathf.CeilToInt(grid.cellSize/distance), unit);
				if (cell == null){
					EndAction(false);
					return;
				}

				fleePos = cell.position;
				unit.MoveTo(fleePos, false);
			}

			var e = unit.lastNavigationEvent;
			if (e != UnitNavigationEventMessage.Event.None){
				EndAction( e == UnitNavigationEventMessage.Event.DestinationReached );
			}
		}

		protected override void OnStop(){
			base.OnStop();
			lastTargetPos = null;
		}
	}
}