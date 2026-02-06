namespace SFA.DAS.Courses.Domain.Courses
{
    public record RelatedOccupation(string Title, int Level)
    {
        public static implicit operator RelatedOccupation(Entities.Standard source)
        {
            if (source == null)
                return null;

            return new RelatedOccupation(source.Title, source.Level);
        }
    }
}
