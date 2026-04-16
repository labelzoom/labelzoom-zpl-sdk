package net.labelzoom.sdk;

/**
 * Exception thrown when a LabelZoom API operation fails.
 */
public class LabelzoomException extends Exception {
    
    /**
     * Constructs a new LabelzoomException with the specified detail message.
     *
     * @param message the detail message
     */
    public LabelzoomException(String message) {
        super(message);
    }
    
    /**
     * Constructs a new LabelzoomException with the specified detail message and cause.
     *
     * @param message the detail message
     * @param cause the cause of the exception
     */
    public LabelzoomException(String message, Throwable cause) {
        super(message, cause);
    }
}

