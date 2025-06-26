namespace DevLife.Application.Features.CodeRoast.Dtos
{
    public class CodeExecutionResultDto
    {
        public bool IsSuccess { get; set; }
        public string? Output { get; set; }
        public string? Error { get; set; }
        public string? ExecutionTime { get; set; }
    }
}