using System.Collections.Generic;
using Assets.Scripts.Factory.Base;
using Assets.Scripts.Factory.UI;
using Assets.Scripts.Grid;
using UnityEngine;

namespace Assets.Scripts.Factory {
    public class FactoryAssembler : FactoryObject {
        [Header("Recipe")]
        public FactoryItem RecipeItem = null;
        public int RecipeItemStored;
        public TextMesh RecipeTextCount = null;

        [Header("Output")]
        public FactoryItem Output = null;
        public int OutputItemsStored;
        public TextMesh OutputTextCount = null;

        [Header("Settings")]
        public int MaxItems = 10;
        public float CraftTime = 1;
        public Transform Slider = null;
        public float SliderScaleX = 2.8f;

        private float _craftingState;
        private bool _leftFirstIn;
        private bool _leftFirstOut;

        private List<FactoryBelt> _inputBelts = new List<FactoryBelt>();
        private FactoryBelt _outputBelt;

        public override void Setup(GridObject gridObject, Direction dir, FactoryNeighbors customNeighbors = null)
        {
            base.Setup(gridObject, dir, customNeighbors);
            FactoryObject northFactory = Neighbors.GetNeighbor(Direction.North);
            if (northFactory != null) {
                FactoryBelt belt = northFactory.GetComponent<FactoryBelt>();
                if (belt != null && belt.Dir == Direction.North) {
                    AddBelt(belt, Dir == Direction.North);
                }
            }
            FactoryObject eastFactory = Neighbors.GetNeighbor(Direction.East);
            if (eastFactory != null) {
                FactoryBelt belt = eastFactory.GetComponent<FactoryBelt>();
                if (belt != null && belt.Dir == Direction.East) {
                    AddBelt(belt, Dir == Direction.East);
                }
            }
            FactoryObject southFactory = Neighbors.GetNeighbor(Direction.South);
            if (southFactory != null) {
                FactoryBelt belt = southFactory.GetComponent<FactoryBelt>();
                if (belt != null && belt.Dir == Direction.South) {
                    AddBelt(belt, Dir == Direction.South);
                }
            }
            FactoryObject westFactory = Neighbors.GetNeighbor(Direction.West);
            if (westFactory != null) {
                FactoryBelt belt = westFactory.GetComponent<FactoryBelt>();
                if (belt != null && belt.Dir == Direction.West) {
                    AddBelt(belt, Dir == Direction.West);
                }
            }
        }

        private void Update()
        {
            if (RecipeItemStored < MaxItems) {
                foreach (FactoryBelt inBelt in _inputBelts) {
                    if (inBelt.LeftOutputSlot.Item == RecipeItem.Item && _leftFirstIn) {
                        inBelt.LeftOutputSlot.RemoveItem();
                        RecipeItemStored++;
                        _leftFirstIn = false;
                    }
                    if (inBelt.RightOutputSlot.Item == RecipeItem.Item && RecipeItemStored < MaxItems) {
                        inBelt.RightOutputSlot.RemoveItem();
                        RecipeItemStored++;
                        _leftFirstIn = true;
                    }
                    if (inBelt.LeftOutputSlot.Item == RecipeItem.Item && RecipeItemStored < MaxItems && !_leftFirstIn) {
                        inBelt.LeftOutputSlot.RemoveItem();
                        RecipeItemStored++;
                        _leftFirstIn = false;
                    }
                }
            }
            if (OutputItemsStored > 0 && _outputBelt != null) {
                if (_outputBelt.LeftInputSlot.Item == null && _leftFirstOut) {
                    _outputBelt.LeftInputSlot.GiveItem(Output.Item);
                    OutputItemsStored--;
                    _leftFirstOut = false;
                }
                if (_outputBelt.RightInputSlot.Item == null && OutputItemsStored > 0) {
                    _outputBelt.RightInputSlot.GiveItem(Output.Item);
                    OutputItemsStored--;
                    _leftFirstOut = true;
                }
                if (_outputBelt.LeftInputSlot.Item == null && OutputItemsStored > 0 && !_leftFirstOut) {
                    _outputBelt.LeftInputSlot.GiveItem(Output.Item);
                    OutputItemsStored--;
                    _leftFirstOut = false;
                }
            }
            if (RecipeItemStored > 0 && OutputItemsStored < MaxItems) {
                _craftingState += Time.deltaTime;
                if (_craftingState > CraftTime) {
                    RecipeItemStored--;
                    OutputItemsStored++;
                    _craftingState = 0;
                }
            }
            if (OutputItemsStored < 0) OutputItemsStored = 0;
            Vector3 s = Slider.localScale;
            s.x = SliderScaleX * _craftingState / CraftTime;
            Slider.localScale = s;
            RecipeTextCount.text = RecipeItemStored.ToString("0");
            OutputTextCount.text = OutputItemsStored.ToString("0");
        }

        public override void OnSelect()
        {
            //FactoryUIController.Instance.SetFactoryObject(this);
        }

        public void AddBelt(FactoryBelt belt, bool output)
        {
            if (output) {
                _outputBelt = belt;
            } else {
                _inputBelts.Add(belt);
            }
        }
    }
}