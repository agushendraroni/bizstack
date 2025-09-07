#!/bin/bash

echo "Testing Auth Service..."

# Build the service
echo "Building auth service..."
dotnet build

if [ $? -eq 0 ]; then
    echo "✅ Auth service builds successfully"
else
    echo "❌ Auth service build failed"
    exit 1
fi

# Test if service can start (quick test)
echo "Testing service startup..."
timeout 10s dotnet run --urls="http://localhost:5001" &
PID=$!

sleep 5

# Check if process is still running
if kill -0 $PID 2>/dev/null; then
    echo "✅ Auth service starts successfully"
    kill $PID
else
    echo "❌ Auth service failed to start"
    exit 1
fi

echo "✅ All auth service tests passed!"