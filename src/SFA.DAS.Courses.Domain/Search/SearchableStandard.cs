using System.Collections.Generic;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Search
{
    public class SearchableStandard
    {
        public SearchableStandard(Standard standard)
        {
            Id = standard.Id;
            Title = standard.Title;
            TypicalJobTitles = standard.TypicalJobTitles;
            Keywords = standard.Keywords;
        }
        
        public int Id { get; }
        public string Title { get; }
        public string TypicalJobTitles { get; }
        public string Keywords { get; }

        public  IEnumerable<IIndexableField> GetFields()
        {
            return new Field[]
            {
                new Int32Field(nameof(Standard.Id), Id, Field.Store.YES),
                new TextField(nameof(Standard.Title), Title ?? "", Field.Store.NO) {Boost = 4.0f},
                new TextField(nameof(Standard.TypicalJobTitles), TypicalJobTitles ?? "", Field.Store.NO) {Boost = 2.0f},
                new TextField(nameof(Standard.Keywords), Keywords ?? "", Field.Store.NO)
            };
        }
    }
}
