const grpc = require("@grpc/grpc-js");

//getters and setters
const protoLoader = require("@grpc/proto-loader");

//load the proto file
const packageDef = protoLoader.loadSync("todo.proto", {});

//load the package definition
const grpcObject = grpc.loadPackageDefinition(packageDef);

//load the todoPackage
const todoPackage = grpcObject.todoPackage;

//create client
const client = new todoPackage.Todo("localhost:40000",
    grpc.credentials.createInsecure());


const text = process.argv[2];

client.createTodo({
    "id": -1,
    "text": text,
}, (err, response) => {
    console.log("Received from server");
    console.log(JSON.stringify(response));
});

client.readTodos({}, (err, response) => {
    console.log("Received from server");
    console.log(JSON.stringify(response));
});

const call = client.readTodosStream();
call.on("data", (item) => {
    console.log("Received item from server");
    console.log(JSON.stringify(item));
});
call.on("end", (e) => console.log("Server done!"));