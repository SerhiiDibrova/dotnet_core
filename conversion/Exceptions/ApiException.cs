package Conversion.Exceptions;

public class ApiException extends RuntimeException {
    private final int statusCode;
    private final String errorMessage;

    public static final int BAD_REQUEST = 400;
    public static final int INTERNAL_SERVER_ERROR = 500;

    public ApiException(int statusCode, String errorMessage) {
        super(errorMessage);
        this.statusCode = statusCode;
        this.errorMessage = errorMessage;
    }

    public int getStatusCode() {
        return statusCode;
    }

    public String getErrorMessage() {
        return errorMessage;
    }

    public static ApiException badRequest(String message) {
        return new ApiException(BAD_REQUEST, message);
    }

    public static ApiException internalServerError(String message) {
        return new ApiException(INTERNAL_SERVER_ERROR, message);
    }
}