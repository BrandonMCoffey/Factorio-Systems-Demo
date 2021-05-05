using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Factory.Base;
using Assets.Scripts.Grid;
using UnityEngine;

namespace Assets.Scripts.Factory {
    public class FactoryInserter : FactoryObject {
        public FactoryItem HeldItem = null;
        public FactoryItem DesiredItem = null;
        [SerializeField] private float _movementSpeed = 0.4f;

        public Direction OutputDirection = Direction.North;
        private InserterConnection _output;
        public Direction InputDirection = Direction.South;
        private InserterConnection _input;

        private Vector3 _basePosition;

        private InserterOperation _operation = InserterOperation.WaitingForItem;

        public override void Setup(GridObject gridObject, Direction dir)
        {
            base.Setup(gridObject, dir);
            FactoryObject outputNeighbor = Neighbors.GetSide(OutputDirection);
            if (outputNeighbor != null) {
                FactoryBelt belt = outputNeighbor.GetComponent<FactoryBelt>();
                if (belt != null) _output = new BeltConnection(belt);
                //FactoryAssembler assembler = outputNeighbor.GetComponent<FactoryAssembler>();
                //if (assembler != null) _output = new AssemblerConnection(assembler);
            }
            FactoryObject inputNeighbor = Neighbors.GetSide(InputDirection);
            if (inputNeighbor != null) {
                FactoryBelt belt = inputNeighbor.GetComponent<FactoryBelt>();
                if (belt != null) _input = new BeltConnection(belt);
                FactoryCreativeOutput creativeOutput = inputNeighbor.GetComponent<FactoryCreativeOutput>();
                if (creativeOutput != null) _input = new CreativeOutputConnection(creativeOutput);
                //FactoryAssembler assembler = inputNeighbor.GetComponent<FactoryAssembler>();
                //if (assembler != null) _input = new AssemblerConnection(assembler);
            }
        }

        private void Start()
        {
            _basePosition = HeldItem.transform.position;
        }

        private void Update()
        {
            if (_input == null || _output == null || HeldItem == null) return;
            if (DesiredItem != null) Debug.Log(_operation + " to " + DesiredItem.gameObject + " at " + DesiredItem.transform.position);
            else Debug.Log(_operation);
            switch (_operation) {
                case InserterOperation.WaitingForItem:
                    if (_input.TestForAvailableItems() && _output.TestForAvailableSlot()) {
                        _operation = InserterOperation.ReachingForItem;
                        DesiredItem = _input.GetDesiredItem(HeldItem.transform.position);
                    }
                    break;
                case InserterOperation.ReachingForItem:
                    if (!_input.TestForAvailableItems() || DesiredItem == null) {
                        _operation = InserterOperation.Returning;
                        break;
                    }
                    HeldItem.transform.position = Vector3.MoveTowards(HeldItem.transform.position, DesiredItem.transform.position, _movementSpeed * Time.deltaTime);
                    float distIn = Vector3.Distance(DesiredItem.transform.position, HeldItem.transform.position);
                    if (distIn < 0.1f) {
                        DesiredItem = _output.GetDesiredItem(HeldItem.transform.position);
                        HeldItem.SetItem(DesiredItem.Item);
                        DesiredItem.RemoveItem();
                        _operation = InserterOperation.MovingItem;
                    } else if (distIn > 2f) {
                        _operation = InserterOperation.Returning;
                    }
                    break;
                case InserterOperation.MovingItem:
                    if (DesiredItem == null || DesiredItem.Item != null) {
                        _operation = InserterOperation.Returning;
                        break;
                    }
                    HeldItem.transform.position = Vector3.MoveTowards(HeldItem.transform.position, DesiredItem.transform.position, _movementSpeed * Time.deltaTime);
                    float distOut = Vector3.Distance(DesiredItem.transform.position, HeldItem.transform.position);
                    if (distOut < 0.1f) {
                        DesiredItem = _output.GetDesiredItem(HeldItem.transform.position);
                        DesiredItem.SetItem(HeldItem.Item);
                        HeldItem.RemoveItem();
                        _operation = InserterOperation.Returning;
                    } else if (distOut > 3f) {
                        _operation = InserterOperation.Returning;
                    }
                    break;
                case InserterOperation.Returning:
                    HeldItem.transform.position = Vector3.MoveTowards(HeldItem.transform.position, _basePosition, _movementSpeed * Time.deltaTime);
                    break;
            }
        }
    }

    internal abstract class InserterConnection {
        public abstract bool TestForAvailableItems();
        public abstract FactoryItem GetDesiredItem(Vector3 pos);
        public abstract bool TestForAvailableSlot();
    }

    internal class BeltConnection : InserterConnection {
        private List<FactoryBeltItem> _beltItems;

        public BeltConnection(FactoryBelt belt)
        {
            _beltItems = new List<FactoryBeltItem>(); // {belt.LeftInputSlot, belt.LeftOutputSlot, belt.RightInputSlot, belt.RightOutputSlot};
            foreach (var slot in belt.OtherSlots) {
                _beltItems.Add(slot);
            }
        }

        public override bool TestForAvailableItems()
        {
            return _beltItems.Any(item => item.Item != null);
        }

        public override FactoryItem GetDesiredItem(Vector3 pos)
        {
            int closest = -1;
            float closestDist = 0;
            for (int i = 0; i < _beltItems.Count; i++) {
                float dist = Vector3.Distance(_beltItems[i].transform.position, pos);
                if (dist > closestDist) {
                    closest = i;
                    closestDist = dist;
                }
            }
            if (closest == -1 || _beltItems[closest] == null) return null;
            return _beltItems[closest];
        }

        public override bool TestForAvailableSlot()
        {
            return _beltItems.Any(item => item.Item == null);
        }
    }

    internal class CreativeOutputConnection : InserterConnection {
        private FactoryItem _item;

        public CreativeOutputConnection(FactoryCreativeOutput creativeOutput)
        {
            _item = creativeOutput.Item;
        }

        public override bool TestForAvailableItems()
        {
            return true;
        }

        public override FactoryItem GetDesiredItem(Vector3 pos)
        {
            return _item != null ? _item : null;
        }

        public override bool TestForAvailableSlot()
        {
            return false;
        }
    }

    /*
    internal class AssemblerConnection : InserterConnection {
        public AssemblerConnection(FactoryAssembler assembler)
        {
        }

        public override bool TestForAvailableItems()
        {
            return false;
        }

        public override Transform GetTransform()
        {
            return null;
        }

        public override bool TestForAvailableSlot()
        {
            return false;
        }
    }
    */

    internal enum InserterOperation {
        WaitingForItem,
        ReachingForItem,
        MovingItem,
        Returning
    }
}