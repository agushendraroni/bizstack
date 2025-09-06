#!/bin/bash

# Script to update all BizStack services to use PostgreSQL

echo "🔄 Updating BizStack services to use PostgreSQL..."

# Services that need PostgreSQL
SERVICES=(
    "user-service"
    "organization-service" 
    "product-service"
    "customer-service"
    "transaction-service"
    "notification-service"
    "file-storage-service"
)

# Update each service
for SERVICE in "${SERVICES[@]}"; do
    echo "📦 Updating $SERVICE..."
    
    # Add Npgsql package
    cd "/mnt/data/Development/LOJIPHIN/BizStack/services/$SERVICE"
    
    # Update csproj to add Npgsql
    if ! grep -q "Npgsql.EntityFrameworkCore.PostgreSQL" "$SERVICE.csproj"; then
        sed -i '/<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" \/>/a\    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.0" />' "$SERVICE.csproj"
        echo "  ✅ Added Npgsql package to $SERVICE"
    fi
    
    # Update Program.cs to use PostgreSQL
    if grep -q "UseInMemoryDatabase" "Program.cs"; then
        # Replace InMemory with PostgreSQL
        sed -i 's/options.UseInMemoryDatabase.*$/options.UseNpgsql(connectionString));/' "Program.cs"
        
        # Add connection string variable
        sed -i '/\/\/ Database/a\var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");' "Program.cs"
        
        # Add auto-migration
        sed -i '/var app = builder.Build();/a\\n\/\/ Auto-migrate database\nusing (var scope = app.Services.CreateScope())\n{\n    var context = scope.ServiceProvider.GetRequiredService<.*DbContext>();\n    context.Database.Migrate();\n}' "Program.cs"
        
        echo "  ✅ Updated Program.cs for $SERVICE"
    fi
done

echo "🎉 All services updated to use PostgreSQL!"
echo "📋 Next steps:"
echo "  1. Run: docker-compose up postgres -d"
echo "  2. Build services: dotnet build"
echo "  3. Run: docker-compose up --build"
