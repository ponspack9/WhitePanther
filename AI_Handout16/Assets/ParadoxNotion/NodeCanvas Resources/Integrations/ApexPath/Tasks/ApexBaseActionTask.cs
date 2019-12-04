using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Apex;
using Apex.Messages;
using Apex.Services;
using Apex.Steering;
using Apex.Units;

namespace NodeCanvas.Tasks.ApexPath{

	[Category("ApexPath")]
	[Icon("ApexPath")]
	abstract public class ApexBaseActionTask : ActionTask<IMovable> {

		protected IUnitFacade unit;

		protected override string OnInit(){
			unit = (agent as Component).GetUnitFacade();
			if (unit == null){
				return "Unable to create Unit Facade";
			}
			return null;
		}

		protected override void OnPause(){OnStop();}
		protected override void OnStop(){
			unit.MoveTo(unit.position, false);
		}
	}
}