
namespace Assets.Scripts.Core.Models
{
    public class BrickModel
    {
        public float Number { get; private set; }
        public float Size { get; private set; }

        public BrickModel(int number, float size)
        {
            Number = number;
            Size = size;
        }

        internal void ChangeSize(float size)
        {
            Size = size;
        }
    }
}
