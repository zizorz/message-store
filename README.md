# message-store
### Requirements
```
.Net Core 2.2
```

### How to run
```
Open in Visual Studio (or similar)
Click Run
A web page with a swagger API should open
```

### How to test with swagger
```
Navigate to /swagger/index.html
Click Authorize in the top right
Type something in the value field to uniquely identify the client
You can now use the API methods through swagger
```

### Example test with swagger
```
1. Authorize with value "client1"
2. Select the POST /api/messages
3. Click "Try it out" in the top right corner of the expanded panel
4. In the body field replace "string" with the message "Hello world!"
5. Click Execute 
6. Check the response body at the bottom and remember the messageId
7. Close the POST /api/messages panel
8. Open the GET /api/messages/{id} panel
9. Click "Try it out" in the top right corner of the expanded panel
10. Type in the messageId you remembered from 6 and click Execute 
11. Check the response to see the message you created earlier
12. To use multiple clients open the swagger page in a different window/tab and authorize again
```