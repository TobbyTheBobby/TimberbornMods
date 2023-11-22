 using System;
 using System.Collections.Generic;
 using System.Linq;


 namespace DifficultySettingsChanger
{
    public class HierarchicalManager
    {
        private readonly DifficultySettingsChangerBox _difficultySettingsChangerBox;

        private readonly List<HierarchicalLayer> _hierarchicalLayers = new();

        public int CurrentIndex => _hierarchicalLayers.Count;
        
        HierarchicalManager(DifficultySettingsChangerBox difficultySettingsChangerBox)
        {
            _difficultySettingsChangerBox = difficultySettingsChangerBox;
        }
        
        public void OpenNewLayer(HierarchicalLayer hierarchicalLayer)
        {
            RemoveUnrelatedToHierarchy(hierarchicalLayer);
            _hierarchicalLayers.Add(hierarchicalLayer);
            _difficultySettingsChangerBox.AddLayer(hierarchicalLayer.VisualElement);
        }

        public void Clear()
        {
            _hierarchicalLayers.Clear();
        }

        private void RemoveUnrelatedToHierarchy(HierarchicalLayer hierarchicalLayer)
        {
            var hierarchicalLayers = _hierarchicalLayers.ToList();
            Plugin.Log.LogInfo(hierarchicalLayer.Index + "");
            foreach (var layer in hierarchicalLayers)
            {
                Plugin.Log.LogWarning(layer.Index + "");
                if (layer.Index >= hierarchicalLayer.Index)
                {
                    _hierarchicalLayers.Remove(layer);
                    try
                    {
                        _difficultySettingsChangerBox.RemoveLayer(layer.VisualElement);
                    }
                    catch (Exception e)
                    {
                        // ignored
                    }
                }
            }
        }
    }
}