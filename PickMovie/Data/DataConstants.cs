namespace PickMovie.Data
{
    public class DataConstants
    {
        public class Movie
        {
            public const int TitleMinLength = 2;
            public const int TitleMaxLength = 30;
            public const int MovieDescriptionMinLength = 20;
            public const int MovieDescriptionMaxLength = 500;
            public const int DirectorNameMinLength = 2;
            public const int DirectorNameMaxLength = 30;
            public const int MovieYearMinValue = 1800;
            public const int MovieYearMaxValue = 2040;
            public const int MinDurationTime = 0;
            public const int MaxDurationTime = 250;
            public const int ActorsMinLength = 4;
            public const int ActorsMaxLength = 50;
        }

        public class User
        {
            public const int UsernameMaxLength = 20;
            public const int UsernameMinLength = 3;
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 20;
            public const string UserEmailRegex = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        }

        public class Critic
        {
            public const int CriticNameMaxLength = 25;

            public const string CriticEmailRegex = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        }
        

        public const int CommentMaxLength = 1000;
    }
}
