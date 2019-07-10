using System;
using System.Collections.Generic;
using System.Text;
using VerticalSliceArchitecture.Core.Domain.Identity;
using VerticalSliceArchitecture.Core.Domain.Shared;
using VerticalSliceArchitecture.Core.Exceptions;
using VerticalSliceArchitecture.Core.Helpers;


namespace VerticalSliceArchitecture.Core.Domain.Games
{
    public class Game :BaseEntity<int>
    {
        public  User User { get; private set; }

        public Guid UserId { get; private set; }
        public string Title { get; private set; }
        public DateTime? ReleaseDate { get; private set; }

        private Game() { }

        public Game(Guid userId,string title,DateTime? releaseDate) : this()
        {
            UserId = userId;
            SetTitle(title);
            SetReleaseDate(releaseDate);
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new DomainException(DomainExceptionCodes.InvalidTitle,
                   ExceptionMessageHelpers.NotEmpty(nameof(Title)));
            }

            if (Title == title)
            {
                return;
            }

            if (title.Length > 255)
            {
                throw new DomainException(DomainExceptionCodes.InvalidTitle,
                   ExceptionMessageHelpers.NoLongerThen(nameof(Title), 255));
            }

            Title = title;
        }
        public void SetReleaseDate(DateTime? releaseDate)
        {           

            if (ReleaseDate == releaseDate)
            {
                return;
            }

            ReleaseDate = releaseDate;
        }
    }
   
}
