# CSC-239-Chat

#Where we are at

At this point, the server is able to detect multi-client users.  
The problem is where the server is unable to process the text from the second client,
but allows the client to be connected to the server.

We need to allow the server to close the text stream once a client enters a message, but reopen when another message is sent.
