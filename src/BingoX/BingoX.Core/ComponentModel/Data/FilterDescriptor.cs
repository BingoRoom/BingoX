﻿namespace BingoX.ComponentModel.Data
{
    /*
        public struct PagingInfo
        {
            public PagingInfo(int index, int size)
            {
                Size = size;
                Index = index;
                //    Sort = new SortFieldCollection();
            }
            public int Size { get; private set; }
            public int Index { get; private set; }
            //   public SortFieldCollection Sort { get; private set; }
        }
        public class SortFieldCollection : Collection<SortField>
        {
            public void AddAsc(string name)
            {
                this.Add(new SortField() { FieldName = name, Sort = Sort.Asc });
            }
            public void AddDesc(string name)
            {
                this.Add(new SortField() { FieldName = name, Sort = Sort.Desc });
            }
        }*/

    public struct FilterDescriptor
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
 
}