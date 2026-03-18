namespace FinanceControl.Application.DTOs;

/// <summary>
/// Resultado de uma operação sem dados (apenas sucesso/erro)
/// </summary>
public record ResultViewModel(
    bool IsSuccess = true, 
    string Message = "", 
    int StatusCode = 200,
    IEnumerable<ValidationError>? Errors = null)
{
    public static ResultViewModel Success(string message = "Operation completed successfully")
        => new(true, message, 200);

    public static ResultViewModel Error(string message, int statusCode = 400)
        => new(false, message, statusCode);

    public static ResultViewModel NotFound(string message = "Resource not found")
        => new(false, message, 404);

    public static ResultViewModel Unauthorized(string message = "Unauthorized")
        => new(false, message, 401);

    public static ResultViewModel Forbidden(string message = "Forbidden")
        => new(false, message, 403);
    
    public static ResultViewModel ValidationError(IEnumerable<ValidationError> errors)
        => new(false, "Validation failed", 422, errors);
}
/// <summary>
/// Resultado de uma operação com dados (genérico)
/// </summary>
public record ResultViewModel<T> : ResultViewModel
{
    public ResultViewModel(
        T? data, 
        bool isSuccess = true, 
        string message = "", 
        int statusCode = 200,
        IEnumerable<ValidationError>? Errors = null) 
        : base(isSuccess, message, statusCode)
    {
        Data = data;
    }

    public T? Data { get; init; }

    public static ResultViewModel<T> Success(T data, string message = "")
        => new(data, true, message, 200);

    public static ResultViewModel<T> Created(T data, string message = "Resource created")
        => new(data, true, message, 201);

    public static ResultViewModel<T> Error(string message, int statusCode = 400)
        => new(default, false, message, statusCode);

    public static ResultViewModel<T> NotFound(string message = "Resource not found")
        => new(default, false, message, 404);

    public static ResultViewModel<T> Unauthorized(string message = "Unauthorized")
        => new(default, false, message, 401);

    public static ResultViewModel<T> Forbidden(string message = "Forbidden")
        => new(default, false, message, 403);

    public static ResultViewModel<T> ValidationError(IEnumerable<ValidationError> errors)
        => new(default, false, "Validation failed", 422, errors);
}

/// <summary>
/// Representa um erro de validação
/// </summary>
public record ValidationError(string Property, string Message);