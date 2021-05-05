using System;
using System.Collections.Generic;
using Assets.Scripts.Factory.Base;
using Assets.Scripts.Grid;
using UnityEngine;

namespace Assets.Scripts.Factory {
    public class FactoryInserter : FactoryObject {
        public FactoryItem HeldItem = null;
        [SerializeField] private float _movementSpeed = 1;

        public Direction OutputDirection = Direction.North;
        private InserterConnection _output;
        public Direction InputDirection = Direction.South;
        private InserterConnection _input;

        private Transform _desiredPosition;
        private Vector3 _basePosition;

        private InserterOperation _operation = InserterOperation.WaitingForItem;

        public override void Setup(GridObject gridObject, Direction dir)
        {
            base.Setup(gridObject, dir);
            FactoryObject outputNeighbor = Neighbors.GetSide(OutputDirection);
            if (outputNeighbor != null) {
                FactoryBelt belt = outputNeighbor.GetComponent<FactoryBelt>();
                if (belt != null) _output = new BeltConnection(belt);
                FactoryCreativeOutput creativeOutput = outputNeighbor.GetComponent<FactoryCreativeOutput>();
                if (creativeOutput != null) _output = new CreativeOutputConnection(creativeOutput);
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

        private void Awake()
        {
            _basePosition = transform.position;
        }

        private void Update()
        {
            if (_input == null || _output == null) return;
            switch (_operation) {
                case InserterOperation.WaitingForItem:
                    if (_input.TestForAvailableItems() && _output.TestForAvailableSlot()) {
                        _operation = InserterOperation.ReachingForItem;
                        _desiredPosition = _input.GetTransform();
                    }
                    break;
                case InserterOperation.ReachingForItem:
                    if (!_input.TestForAvailableItems()) {
                        _operation = InserterOperation.Returning;
                        break;
                    }
                    transform.position = Vector3.Lerp(transform.position, _desiredPosition.position, _movementSpeed);
                    float dist = Vector3.Distance(_desiredPosition.position, transform.position);
                    if (dist < 0.1f) {
                        _operation = InserterOperation.MovingItem;
                        _desiredPosition = _output.GetTransform();
                    } else if (dist > 1f) {
                        _operation = InserterOperation.Returning;
                    }
                    break;
                case InserterOperation.MovingItem:
                    break;
                case InserterOperation.Returning:
                    transform.position = Vector3.Lerp(transform.position, _basePosition, _movementSpeed);
                    break;
            }
        }
    }

    internal abstract class InserterConnection {
        public abstract bool TestForAvailableItems();
        public abstract Transform GetTransform();
        public abstract bool TestForAvailableSlot();
    }

    internal class BeltConnection : InserterConnection {
        private List<FactoryBeltItem> _beltItems;

        public BeltConnection(FactoryBelt belt)
        {
            _beltItems = new List<FactoryBeltItem>();
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

        public override Transform GetTransform()
        {
            return _item != null ? _item.transform : null;
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