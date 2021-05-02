using System;
using System.Collections.Generic;
using Assets.Scripts.Factory.Base;
using Assets.Scripts.Grid;
using UnityEngine;

namespace Assets.Scripts.Factory {
    public class FactoryBelt : FactoryObject {
        [Header("Settings (Assume Belt Rotated to North)")]
        public Direction OutputDirection = Direction.North;
        public FactoryBeltItem LeftOutputSlot = null;
        public FactoryBeltItem RightOutputSlot = null;
        public Direction InputDirection = Direction.South;
        public FactoryBeltItem LeftInputSlot = null;
        public FactoryBeltItem RightInputSlot = null;

        public override void Setup(GridObject gridObject, Direction dir)
        {
            base.Setup(gridObject, dir);
            FactoryBelt frontBelt = Neighbors.GetSide(OutputDirection)?.GetComponent<FactoryBelt>();
            if (frontBelt != null && MatchesIO(frontBelt.InputDirection, frontBelt.Dir, OutputDirection)) {
                LeftOutputSlot.NextPosition = frontBelt.LeftInputSlot;
                frontBelt.LeftInputSlot.PreviousPosition = LeftOutputSlot;
                RightOutputSlot.NextPosition = frontBelt.RightInputSlot;
                frontBelt.RightInputSlot.PreviousPosition = RightOutputSlot;
            }
            FactoryBelt backBelt = Neighbors.GetSide(InputDirection)?.GetComponent<FactoryBelt>();
            if (backBelt != null && MatchesIO(backBelt.OutputDirection, backBelt.Dir, InputDirection)) {
                backBelt.LeftOutputSlot.NextPosition = LeftInputSlot;
                LeftInputSlot.PreviousPosition = backBelt.LeftOutputSlot;
                backBelt.RightOutputSlot.NextPosition = RightInputSlot;
                RightInputSlot.PreviousPosition = backBelt.RightOutputSlot;
            }
            FactoryCreativeOutput creativeOutput = Neighbors.GetSide(InputDirection)?.GetComponent<FactoryCreativeOutput>();
            if (creativeOutput != null) {
                creativeOutput.AddBelt(this);
            }
        }

        public override void OnBreak()
        {
            LeftInputSlot.OnBreakInput();
            RightInputSlot.OnBreakInput();
            LeftOutputSlot.OnBreakOutput();
            RightOutputSlot.OnBreakOutput();
            base.OnBreak();
        }

        public bool MatchesIO(Direction dir, Direction rot, Direction IO)
        {
            int num = (DirectionToInt(dir) + DirectionToInt(rot)) % 4;
            int io = (DirectionToInt(Dir) + DirectionToInt(IO) + 2) % 4;
            return num == io;
        }

        public int DirectionToInt(Direction dir)
        {
            return dir switch
            {
                Direction.North => 0,
                Direction.East  => 1,
                Direction.South => 2,
                Direction.West  => 3,
                _               => 0
            };
        }
    }
}