using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core.Models;

namespace Assets.Scripts.Core
{
    public class BricksTower
    {
        private readonly List<BrickModel> _bricks;

        private readonly float _maxSizeOfBrick;
        private readonly float _decreaseOfBottomPercent = 0.8f;
        private readonly float _increaseValueByPerfectMove = 0.2f;
        private readonly float _prefectMoveFault = 0.05f;

        private int _lastPerfectBrickIndex;
        private int _counter = -1;
        private bool _isGameOver;

        public BricksTower(float maxSize, int capacity = 0)
        {
            if (capacity > 0)
            {
                _bricks = new List<BrickModel>(capacity);
            }
            else
            {
                _bricks = new List<BrickModel>();
            }

            _maxSizeOfBrick = maxSize;
        }

        public IEnumerable Bricks
        {
            get { return _bricks; }
        }

        public float LastBrickSize
        {
            get
            {
                if (_counter > 0)
                    return _bricks[_counter].Size;

                return 1;
            }
        }

        public bool IsValidSize(float size)
        {
            if (_counter < 0)
                return size <= _maxSizeOfBrick;

            return size <= _bricks[_counter].Size;
        }

        public bool IsLastBrickPerfetcMove()
        {
            return !_isGameOver && _lastPerfectBrickIndex == _counter && _counter != 0;
        }

        public BrickModel BuildNewBrick(float size)
        {
            if (_isGameOver || !IsValidSize(size))
            {
                _isGameOver = true;
                return null;
            }

            if (_counter < 0)
            {
                _lastPerfectBrickIndex = _counter = 0;
                _bricks.Add(new BrickModel(_counter, size));
            }
            else
            {
                var bottomBrickSize = _bricks[_counter].Size;

                if (bottomBrickSize - _prefectMoveFault <= size && size <= bottomBrickSize)
                {
                    var newBrickSize = size + _increaseValueByPerfectMove;
                    _bricks.Add(new BrickModel(++_counter, newBrickSize > _maxSizeOfBrick ? _maxSizeOfBrick : newBrickSize))
;
                    RescaleBottomBricks();

                    _lastPerfectBrickIndex = _counter;
                }
                else
                {
                    _bricks.Add(new BrickModel(++_counter, size));
                }
            }

            return _bricks[_counter];
        }

        private void RescaleBottomBricks()
        {
            for (int i = _lastPerfectBrickIndex; i < _counter; i++)
            {
                var newSize = _bricks[i].Size * _decreaseOfBottomPercent;

                _bricks[i].ChangeSize(newSize <= _maxSizeOfBrick ?
                    (newSize < 0.1f ? 0.1f : newSize) : _maxSizeOfBrick);
            }
        }
    }
}
