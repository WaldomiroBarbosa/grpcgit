import seminar_pb2_grpc
import seminar_pb2

import grpc

def run():
    with grpc.insecure_channel('localhost:5274') as channel:
        stub = seminar_pb2_grpc.SeminarStub(channel)
        print("Please type the RPC call number (1, 2, or 3):")
        rpccall = input()

        if rpccall == "1":
            numbera = int(input("Enter the first number: "))
            numberb = int(input("Enter the second number: "))
            operation = int(input("Enter the operation type (+, -, *, /): "))
            numberOperands = seminar_pb2.NumberOperands(first_op=numbera, second_op=numberb, op_type=operation)
            operationResult = stub.Calculate(numberOperands)
            print(f"Operation result: {operationResult.result}")
        
        elif rpccall == "2":
            text = input("Enter the string to transform: ")
            stringMessage = seminar_pb2.StringMessage(input=text)
            stringResult = stub.TransformString(stringMessage)
            print(f"Transformed string: {stringResult.output}")
        
        elif rpccall == "3":
            text = input("Enter the content to process for file operation: ")
            fileOperationRequest = seminar_pb2.FileOperationRequest(content=text)
            fileOperationResult = stub.FileOperationResult(fileOperationRequest)
            print(f"File operation result: {fileOperationResult.result}")
        
        else:
            print("Invalid RPC call number.")

if __name__ == "__main__":
    run()