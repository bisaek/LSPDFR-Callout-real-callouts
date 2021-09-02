using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using Rage.Native;
using System;
using System.Drawing;

namespace realCallouts.Callouts
{
    [CalloutInfo("Call Template", CalloutProbability.High)]
    public class Template : Callout
    {
        //Private References

        public override bool OnBeforeCalloutDisplayed()
        {

            //Last Line
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {

            //Last Line
            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            //First Line
            base.OnCalloutNotAccepted();

        }

        public override void Process()
        {
            //First Line
            base.Process();
        }

        public override void End()
        {
            //First Line
            base.End();
        }
    }
}
