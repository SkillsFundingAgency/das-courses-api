namespace SFA.DAS.Courses.Domain.Courses
{
    public record RelatedOccupation(string Title, int Level)
    {
        public static implicit operator RelatedOccupation(Entities.Standard standard)
        {
            return new RelatedOccupation(standard.Title, standard.Level);
        }
    }
}
