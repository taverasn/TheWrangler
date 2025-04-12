using Synty.SidekickCharacters.API;
using Synty.SidekickCharacters.Database;
using Synty.SidekickCharacters.Enums;
using Synty.SidekickCharacters.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Synty.SidekickCharacters.Demo
{
    /// <summary>
    ///     An example script to show how to interact with the Sidekick API in regards to parts at runtime.
    /// </summary>
    public class RuntimePartsDemo : MonoBehaviour
    {
        private readonly string _OUTPUT_MODEL_NAME = "Sidekick Character";

        Dictionary<CharacterPartType, int> _partIndexDictionary = new Dictionary<CharacterPartType, int>();
        Dictionary<CharacterPartType, Dictionary<string, string>> _availablePartDictionary = new Dictionary<CharacterPartType, Dictionary<string, string>>();

        private DatabaseManager _dbManager;
        private SidekickRuntime _sidekickRuntime;

        private Dictionary<CharacterPartType, Dictionary<string, string>> _partLibrary;

        /// <inheritdoc cref="Start"/>
        void Start()
        {
            // Create a new instance of the database manager to access database content.
            _dbManager = new DatabaseManager();

            // Load the base model and material required to create an instance of the Sidekick Runtime API.
            GameObject model = Resources.Load<GameObject>("Meshes/SK_BaseModel");
            Material material = Resources.Load<Material>("Materials/M_BaseMaterial");

            _sidekickRuntime = new SidekickRuntime(model, material, null, _dbManager);

            // Populate the parts list for easy access.
            _partLibrary = _sidekickRuntime.PartLibrary;

            // For this example we are only interested in Upper Body parts, so we filter the list of all parts to only get the ones we want.
            List<CharacterPartType> upperBodyParts = PartGroup.UpperBody.GetPartTypes();

            foreach (CharacterPartType type in upperBodyParts)
            {
                _availablePartDictionary.Add(type, _partLibrary[type]);
                _partIndexDictionary.Add(type, Random.Range(0, _availablePartDictionary[type].Count - 1));
            }

            UpdateModel();
        }

        /// <summary>
        ///     Handles the click on the forward button for the Torso part.
        /// </summary>
        public void ForwardTorso()
        {
            int index = _partIndexDictionary[CharacterPartType.Torso];
            index++;
            if (index >= _availablePartDictionary[CharacterPartType.Torso].Count)
            {
                index = 0;
            }

            _partIndexDictionary[CharacterPartType.Torso] = index;
            UpdateModel();
        }

        /// <summary>
        ///     Handles the click on the backward button for the Torso part.
        /// </summary>
        public void BackwardTorso()
        {
            int index = _partIndexDictionary[CharacterPartType.Torso];
            index--;
            if (index < 0)
            {
                index = _availablePartDictionary[CharacterPartType.Torso].Count - 1;
            }

            _partIndexDictionary[CharacterPartType.Torso] = index;
            UpdateModel();
        }

        /// <summary>
        ///     Handles the click on the forward button for the ArmUpperLeft part.
        /// </summary>
        public void ForwardUpperArmLeft()
        {
            int index = _partIndexDictionary[CharacterPartType.ArmUpperLeft];
            index++;
            if (index >= _availablePartDictionary[CharacterPartType.ArmUpperLeft].Count)
            {
                index = 0;
            }

            _partIndexDictionary[CharacterPartType.ArmUpperLeft] = index;
            UpdateModel();
        }

        /// <summary>
        ///     Handles the click on the backward button for the ArmUpperLeft part.
        /// </summary>
        public void BackwardUpperArmLeft()
        {
            int index = _partIndexDictionary[CharacterPartType.ArmUpperLeft];
            index--;
            if (index < 0)
            {
                index = _availablePartDictionary[CharacterPartType.ArmUpperLeft].Count - 1;
            }

            _partIndexDictionary[CharacterPartType.ArmUpperLeft] = index;
            UpdateModel();
        }

        /// <summary>
        ///     Handles the click on the forward button for the ArmUpperRight part.
        /// </summary>
        public void ForwardUpperArmRight()
        {
            int index = _partIndexDictionary[CharacterPartType.ArmUpperRight];
            index++;
            if (index >= _availablePartDictionary[CharacterPartType.ArmUpperRight].Count)
            {
                index = 0;
            }

            _partIndexDictionary[CharacterPartType.ArmUpperRight] = index;
            UpdateModel();
        }

        /// <summary>
        ///     Handles the click on the backward button for the ArmUpperRight part.
        /// </summary>
        public void BackwardUpperArmRight()
        {
            int index = _partIndexDictionary[CharacterPartType.ArmUpperRight];
            index--;
            if (index < 0)
            {
                index = _availablePartDictionary[CharacterPartType.ArmUpperRight].Count - 1;
            }

            _partIndexDictionary[CharacterPartType.ArmUpperRight] = index;
            UpdateModel();
        }

        /// <summary>
        ///     Handles the click on the forward button for the ArmLowerLeft part.
        /// </summary>
        public void ForwardLowerArmLeft()
        {
            int index = _partIndexDictionary[CharacterPartType.ArmLowerLeft];
            index++;
            if (index >= _availablePartDictionary[CharacterPartType.ArmLowerLeft].Count)
            {
                index = 0;
            }

            _partIndexDictionary[CharacterPartType.ArmLowerLeft] = index;
            UpdateModel();
        }

        /// <summary>
        ///     Handles the click on the backward button for the ArmLowerLeft part.
        /// </summary>
        public void BackwardLowerArmLeft()
        {
            int index = _partIndexDictionary[CharacterPartType.ArmLowerLeft];
            index--;
            if (index < 0)
            {
                index = _availablePartDictionary[CharacterPartType.ArmLowerLeft].Count - 1;
            }

            _partIndexDictionary[CharacterPartType.ArmLowerLeft] = index;
            UpdateModel();
        }

        /// <summary>
        ///     Handles the click on the forward button for the ArmLowerRight part.
        /// </summary>
        public void ForwardLowerArmRight()
        {
            int index = _partIndexDictionary[CharacterPartType.ArmLowerRight];
            index++;
            if (index >= _availablePartDictionary[CharacterPartType.ArmLowerRight].Count)
            {
                index = 0;
            }

            _partIndexDictionary[CharacterPartType.ArmLowerRight] = index;
            UpdateModel();
        }

        /// <summary>
        ///     Handles the click on the backward button for the ArmLowerRight part.
        /// </summary>
        public void BackwardLowerArmRight()
        {
            int index = _partIndexDictionary[CharacterPartType.ArmLowerRight];
            index--;
            if (index < 0)
            {
                index = _availablePartDictionary[CharacterPartType.ArmLowerRight].Count - 1;
            }

            _partIndexDictionary[CharacterPartType.ArmLowerRight] = index;
            UpdateModel();
        }

        /// <summary>
        ///     Handles the click on the forward button for the HandLeft part.
        /// </summary>
        public void ForwardHandLeft()
        {
            int index = _partIndexDictionary[CharacterPartType.HandLeft];
            index++;
            if (index >= _availablePartDictionary[CharacterPartType.HandLeft].Count)
            {
                index = 0;
            }

            _partIndexDictionary[CharacterPartType.HandLeft] = index;
            UpdateModel();
        }

        /// <summary>
        ///     Handles the click on the backward button for the HandLeft part.
        /// </summary>
        public void BackwardHandLeft()
        {
            int index = _partIndexDictionary[CharacterPartType.HandLeft];
            index--;
            if (index < 0)
            {
                index = _availablePartDictionary[CharacterPartType.HandLeft].Count - 1;
            }

            _partIndexDictionary[CharacterPartType.HandLeft] = index;
            UpdateModel();
        }

        /// <summary>
        ///     Handles the click on the forward button for the HandRight part.
        /// </summary>
        public void ForwardHandRight()
        {
            int index = _partIndexDictionary[CharacterPartType.HandRight];
            index++;
            if (index >= _availablePartDictionary[CharacterPartType.HandRight].Count)
            {
                index = 0;
            }

            _partIndexDictionary[CharacterPartType.HandRight] = index;
            UpdateModel();
        }

        /// <summary>
        ///     Handles the click on the backward button for the HandRight part.
        /// </summary>
        public void BackwardHandRight()
        {
            int index = _partIndexDictionary[CharacterPartType.HandRight];
            index--;
            if (index < 0)
            {
                index = _availablePartDictionary[CharacterPartType.HandRight].Count - 1;
            }

            _partIndexDictionary[CharacterPartType.HandRight] = index;
            UpdateModel();
        }

        /// <summary>
        ///     Handles the click on the forward button for the AttachmentBack part.
        /// </summary>
        public void ForwardBackAttachment()
        {
            int index = _partIndexDictionary[CharacterPartType.AttachmentBack];
            index++;
            if (index >= _availablePartDictionary[CharacterPartType.AttachmentBack].Count)
            {
                index = 0;
            }

            _partIndexDictionary[CharacterPartType.AttachmentBack] = index;
            UpdateModel();
        }

        /// <summary>
        ///     Handles the click on the backward button for the AttachmentBack part.
        /// </summary>
        public void BackwardBackAttachment()
        {
            int index = _partIndexDictionary[CharacterPartType.AttachmentBack];
            index--;
            if (index < 0)
            {
                index = _availablePartDictionary[CharacterPartType.AttachmentBack].Count - 1;
            }

            _partIndexDictionary[CharacterPartType.AttachmentBack] = index;
            UpdateModel();
        }

        /// <summary>
        ///     Updates the body size blends based on the slider values.
        /// </summary>
        /// <param name="slider">The UI slider to get the values from.</param>
        public void UpdateBodySize(Slider slider)
        {
            // If the slider is greater than 0, then we update the Heavy blend and zero the Skinny.
            if (slider.value > 0)
            {
                _sidekickRuntime.BodySizeHeavyBlendValue = slider.value;
                _sidekickRuntime.BodySizeSkinnyBlendValue = 0;
            }
            // If the slider is 0 or below, we zero the Heavy blend, then we update the Skinny blend.
            else
            {
                _sidekickRuntime.BodySizeHeavyBlendValue = 0;
                _sidekickRuntime.BodySizeSkinnyBlendValue = -slider.value;
            }

            UpdateModel();
        }

        /// <summary>
        ///     Updates the created character model.
        /// </summary>
        private void UpdateModel()
        {
            // Create and populate the list of parts to use from the parts list.
            List<SkinnedMeshRenderer> partsToUse = new List<SkinnedMeshRenderer>();

            foreach (KeyValuePair<CharacterPartType, Dictionary<string, string>> entry in _availablePartDictionary)
            {
                int index = _partIndexDictionary[entry.Key];
                string path = entry.Value.Values.ToArray()[index];
                string resource = GetResourcePath(path);
                GameObject partContainer = Resources.Load<GameObject>(resource);
                partsToUse.Add(partContainer.GetComponentInChildren<SkinnedMeshRenderer>());
            }

            // Check for an existing copy of the model, if it exists, delete it so that we don't end up with duplicates.
            GameObject character = GameObject.Find(_OUTPUT_MODEL_NAME);

            if (character != null)
            {
                Destroy(character);
            }

            // Create a new character using the selected parts using the Sidekicks API.
            character = _sidekickRuntime.CreateCharacter(_OUTPUT_MODEL_NAME, partsToUse, false, true);
        }

        /// <summary>
        ///     Gets a resource path for using with Resources.Load() from a full path.
        /// </summary>
        /// <param name="fullPath">The full path to get the resource path from.</param>
        /// <returns>The resource path.</returns>
        private string GetResourcePath(string fullPath)
        {
            string directory = Path.GetDirectoryName(fullPath);
            int startIndex = directory.IndexOf("Resources") + 10;
            directory = directory.Substring(startIndex, directory.Length - startIndex);
            return Path.Combine(directory, Path.GetFileNameWithoutExtension(fullPath));
        }
    }
}
