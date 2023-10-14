const grpc = require("@grpc/grpc-js");

//getters and setters
const protoLoader = require("@grpc/proto-loader");

//load the proto file
const packageDef = protoLoader.loadSync("todo.proto", {});

//load the package definition
const grpcObject = grpc.loadPackageDefinition(packageDef);

//load the todoPackage
const todoPackage = grpcObject.todoPackage;

//create a server
const server = new grpc.Server();

//http2 needs a secure connection
//creating insecure connection
//can be set ssl certificates if needed
//grpc.ServerCredentials.createSsl(rootCerts, keyCertPairs, checkClientCertificate)
server.bindAsync("0.0.0.0:40000", grpc.ServerCredentials.createInsecure());
server.addService(todoPackage.Todo.service, {
  "createTodo": createTodo,
  "readTodos": readTodos,
  "readTodosStream": readTodosStream,
});

server.start();

const todos = [];

//map of todos
//needs to be a map because we need to have a key
//call and callback are the parameters
function createTodo(call, callback) {
  let todoItem = {
    "id": todos.length + 1,
    "text": call.request.text,
  }
  todos.push(todoItem);
  callback(null, todoItem);
}

function readTodos(call, callback) {
  callback(null, { "items": todos });
}

function readTodosStream(call, callback) {
  todos.forEach(t => call.write(t));
  call.end();
}

