using MSCLoader;
using UnityEngine;

namespace DetectPlayerLookingAtObjectExample
{
    public class DetectPlayerLookingAtObjectExample : Mod
    {
        public override string ID => "DetectPlayerLookingAtObjectExample"; //Your mod ID (unique)
        public override string Name => "Detect Player Looking At Object Example"; //You mod name
        public override string Author => "tommojphillips"; //Your Username
        public override string Version => "1.0"; //Version
        public override bool UseAssetsFolder => true;

        private float maxDistance = 5;
        private GameObject truckEngine;
        private Texture2D engineTexture;

        public override void OnLoad()
        {
            // Written, 23.09.2018
            // Called once, when mod is loading after game is fully loaded

            // Creating Game Object.
            this.truckEngine = LoadAssets.LoadOBJ(this, @"truck_engine.obj", true, true);
            // Loading and assigning the texture for the object.
            this.engineTexture = LoadAssets.LoadTexture(this, "Truck_Engine_Texture.png");
            this.truckEngine.GetComponent<Renderer>().material.mainTexture = this.engineTexture;
            // Naming the object. NOTE, name must follow naming convention of <ObjectName>(xxxxx) where <ObjectName> is the game objects name.
            this.truckEngine.name = "Truck Engine(xxxxx)";
            // Spawning the game object to home.
            this.truckEngine.transform.position = new Vector3(-20.7f, 10, 10.9f); // Home Location (Outside Garage)
            this.truckEngine.transform.rotation = new Quaternion(0, 90, 90, 0); // The Rotation.
            // Allowing the object to be picked up.
            this.truckEngine.tag = "PART";
            // Sending object to it's own layer.
            this.truckEngine.layer = LayerMask.NameToLayer(this.truckEngine.name);
        }        

        public override void Update()
        {
            // Written, 23.09.2018
            // Update is called once per frame

            RaycastHit hitInfo;
            bool hasHit = (Physics.Raycast(
                Camera.main.ScreenPointToRay(Input.mousePosition), // Where the camera is facing.
                out hitInfo, // The hit info. 
                this.maxDistance, // The distance to search in.
                1 << this.truckEngine.layer)// The layer to search in. 
                && hitInfo.transform.gameObject == this.truckEngine); // checking if raycast hit said gameobject

            if (hasHit)
            {
                ModConsole.Print("has detected: " + hitInfo.transform.gameObject.name); // Prints the game object's name to the console when the player is looking at it. Expecting 'Truck Engine(xxxxx)'.
                PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true; // Enables the hand-symbol.
                PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "testing 123"; // Setting the text.
            }
            else
            {
                if (PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value)
                {
                    PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = false; // Disables the hand-symbol.
                    PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = ""; // Resetting text.
                }
            }
        }
    }
}
