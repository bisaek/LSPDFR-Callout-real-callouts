using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;

namespace realCallouts.Callouts
{
    [CalloutInfo("Stolen Construction Equipment", CalloutProbability.Medium)]
    public class ConstructionEquipment : Callout
    {
        //Private References
        private Ped Suspect;

        private Vehicle SuspectVehicle;
        private Vehicle SuspectTrailer;

        private Vector3 SpawnPoint;

        private Blip SuspectBlip;

        private LHandle Pursuit;

        private bool PursuitCreated = false;


        public override bool OnBeforeCalloutDisplayed()
        {
            //We create the spawn point
            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(250f));
            Game.LogTrivial("Spawn point created");

            //Check the minimum distance from the player
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 30f); AddMinimumDistanceCheck(20f, SpawnPoint);

            //Create the callout message
            CalloutMessage = "Stolen Construction Equipment"; CalloutPosition = SpawnPoint;

            //Play the scanner audio
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_GRAND_THEFT_AUTO IN_OR_ON_POSITION", SpawnPoint);

            //Create friendly name that appears on LCPDFR.com
            FriendlyName = "stolen construction equipment";

            //Last Line
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            //Create random list of cars
            var randomcar = new Random();
            var modellist = new List<string> { "FORKLIFT", "­utillitruck", "­utillitruck2", "­utillitruck3", "burrito4", "boxville3", "bison2", "bison3", "dump", "bulldozer", "mixer2", "rubble" };
            int index = randomcar.Next(modellist.Count);

            //Select the random vehicle
            SuspectVehicle = new Vehicle(modellist[index], SpawnPoint);

            //Make the vehicle persistent
            SuspectVehicle.IsPersistent = true;

            //Add trailers if certain vehicles
            if (modellist[index] == "bison2")
            {
                SuspectTrailer = new Vehicle("TRAILERSMALL", SpawnPoint);
                SuspectTrailer.IsPersistent = true;

                SuspectVehicle.Trailer = SuspectTrailer;
            }

            if (modellist[index] == "bison3")
            {
                SuspectTrailer = new Vehicle("TRAILERSMALL", SpawnPoint);
                SuspectTrailer.IsPersistent = true;

                SuspectVehicle.Trailer = SuspectTrailer;
            }

            //Create a random driver
            Suspect = SuspectVehicle.CreateRandomDriver();
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;

            //Create a blip
            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.IsFriendly = false;

            //Make driver drive away
            Suspect.Tasks.CruiseWithVehicle(20f, VehicleDrivingFlags.Emergency);

            //Last Line
            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            //First Line
            base.OnCalloutNotAccepted();
            if (Suspect.Exists()) { Suspect.Dismiss(); }
            if (SuspectVehicle.Exists()) { SuspectVehicle.Dismiss(); }
            if (SuspectTrailer.Exists()) { SuspectTrailer.Dismiss(); }
            if (SuspectBlip.Exists()) { SuspectBlip.Delete(); }

        }

        public override void Process()
        {
            //First Line
            base.Process();

            //Create a pursuit if it is not already created and when the player is within 30 meters
            if (!PursuitCreated && Game.LocalPlayer.Character.DistanceTo(Suspect.Position) < 30f)
            {
                Pursuit = Functions.CreatePursuit();
                Functions.AddPedToPursuit(Pursuit, Suspect);
                Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                PursuitCreated = true;
            }

            //End the callout if the pursuit was created and is no longer running
            if (PursuitCreated && !Functions.IsPursuitStillRunning(Pursuit))
            {
                End();
            }
        }

        public override void End()
        {
            //First Line
            base.End();
            if (Suspect.Exists()) { Suspect.Dismiss(); }
            if (SuspectVehicle.Exists()) { SuspectVehicle.Dismiss(); }
            if (SuspectTrailer.Exists()) { SuspectTrailer.Dismiss(); }
            if (SuspectBlip.Exists()) { SuspectBlip.Delete(); }
        }
    }
}