package Exceptions;

import java.util.List;
import javax.ws.rs.core.Response;

public class ValidationException extends Exception {
    private List<String> errorDetails;

    public ValidationException(List<String> errorDetails) {
        super("Validation errors occurred: " + String.join(", ", errorDetails));
        this.errorDetails = errorDetails;
    }

    public List<String> getErrorDetails() {
        return errorDetails;
    }

    public Response toResponse() {
        return Response.status(Response.Status.BAD_REQUEST)
                       .entity(getErrorDetails())
                       .build();
    }
}