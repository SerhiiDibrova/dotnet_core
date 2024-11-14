package Exceptions;

public class ApiException extends Exception {
    private String errorMessage;
    private int statusCode;

    public ApiException(String errorMessage, int statusCode) {
        super("An error occurred: " + errorMessage);
        this.errorMessage = errorMessage;
        this.statusCode = statusCode;
    }

    public String getErrorMessage() {
        return errorMessage;
    }

    public int getStatusCode() {
        return statusCode;
    }

    public static ApiException internalServerError() {
        return new ApiException("An unexpected error occurred.", 500);
    }

    public static ApiException structuredErrorResponse(String errorMessage, int statusCode) {
        return new ApiException(errorMessage, statusCode);
    }
}