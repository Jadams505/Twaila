using System;
using System.ComponentModel;
using Twaila.Systems;

namespace Twaila.Config
{
    public class ContextIndex
    {
        [DefaultValue(0)]
        public int Index;

        public ContextIndex()
        {
            Index = 0;
        }

        public void SetIndex(int index)
        {
            if(ContextSystem.Instance != null)
                Index = Math.Clamp(index, 0, ContextSystem.Instance.ContextEntries.Count - 1);
        }

        public override bool Equals(object obj)
        {
            return obj is ContextIndex index &&
                   Index == index.Index;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Index);
        }

        public override string ToString()
        {
            if (ContextSystem.Instance == null)
                return Index.ToString();

            if (Index < 0 || Index >= ContextSystem.Instance.ContextEntries.Count)
                return "Invalid Index";

            return $"{Index} ({ContextSystem.Instance.ContextEntries[Index].Name.Value})";
        }
    }
}
