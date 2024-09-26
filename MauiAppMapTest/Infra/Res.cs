namespace MauiAppMapTest.Infra
{

    public class Res<T>
    {
        public bool IsSuccess { get; init; } = false;
        public T? Data { get; set; }
        public ErrorResponse ErrorList { get; set; }
        public string? ErrorMsg
        {
            get => ErrorList.Messages.FirstOrDefault();
        }

        public Res(T data, bool isSuccess = true)
        {
            Data = data;
            IsSuccess = isSuccess;
        }

        public Res(string errorText)
        {
            IsSuccess = false;
            ErrorList = new ErrorResponse() { Messages = [errorText] };
        }

        public Res(IEnumerable<string> errorTexts)
        {
            IsSuccess = false;
            ErrorList = new ErrorResponse() { Messages = new List<string>(errorTexts.ToList()) };
        }

        public Res()
        {
            IsSuccess = false;
        }
    }
}
