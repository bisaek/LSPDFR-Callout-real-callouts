using System;
using System.Drawing;
using Rage;
using Rage.Native;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace realCallouts.Callouts
{
    [CalloutInfo("Call Template", CalloutProbability.High)]
    public class personWithKnife : Callout
    {
        //Private References

        private Vector3 SpawnPoint;
        private Ped suspect;
        private bool suspectActivated = false;
        private int chanceToSurrender = 30;
        private bool surrender;
        private LHandle pursuit;

        public override bool OnBeforeCalloutDisplayed()
        {
            //Create SpawnPoint
            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(250f));
            
            //Add Code to be done before displayed to player here
            
            
            
            
            
            
            //Create the callout message
            CalloutMessage = "person with knife";
            
            //Set the callout position
            CalloutPosition = SpawnPoint;
            
            //LCPDFR.com Friendly Name
            FriendlyName = "person with knife";
            
            //Last Line
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            suspect = new Ped(SpawnPoint);
            suspect.Inventory.GiveNewWeapon(WeaponHash.HeavySniper, 1, true);

            surrender = chanceToSurrender <= new Random().Next(101);
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

            // activate suspect
            if (!suspectActivated && Game.LocalPlayer.Character.DistanceTo(suspect) <= 30f)
            {
                if (surrender)
                {
                    suspect.Tasks.PutHandsUp(-1, Game.LocalPlayer.Character);
                }
                else
                {
                    pursuit = Functions.CreatePursuit();
                }
                suspectActivated = true;
            }

            // run end
            if (Game.LocalPlayer.Character.IsDead) End();
            if ()
        }

        public override void End()
        {
            //First Line
            base.End();
        }
    }
}