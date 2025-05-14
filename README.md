# 🖼️ AI-Powered Image Gallery (Backend)

This is the backend API for an AI-enhanced image gallery built with **.NET 8**, **Azure**, and **Cognitive Services**. It allows users to upload images, automatically analyzes them using Azure's Computer Vision API, and stores the results in Azure Blob Storage and Cosmos DB.

## 🚀 Features

- Upload image files to Azure Blob Storage
- Automatically analyze images using Computer Vision (OCR and tags)
- Store image metadata (description, tags, text, etc.) in Azure Cosmos DB
- Search images by tags or extracted text
- Designed for integration with an Angular front end (coming soon)

---

## 🛠️ Tech Stack

- **.NET 8 Web API**
- **Azure Blob Storage** for file storage
- **Azure Cosmos DB** (NoSQL) for metadata
- **Azure AI Vision API** for analysis (OCR and tagging)
- **Swagger** for API testing

---

## 🧩 Configuration

This project uses [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) to store sensitive credentials during development.

### 🔐 Required Configuration (via User Secrets)

```bash
dotnet user-secrets init
dotnet user-secrets set "Azure:BlobStorage:ConnectionString" "<your_blob_connection_string>"
dotnet user-secrets set "Azure:CosmosDb:ConnectionString" "<your_cosmosdb_connection_string>"
dotnet user-secrets set "Azure:ComputerVision:Endpoint" "<your_computer_vision_endpoint>"
dotnet user-secrets set "Azure:ComputerVision:Key" "<your_computer_vision_key>"
```

## 📁 appsettings.json
Keep this file in the repo, with placeholders:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Azure": {
    "BlobStorage": {
      "ConnectionString": "Use user-secrets or environment variables to retrieve",
      "ContainerName": "Use user-secrets or environment variables to retrieve"
    },
    "CosmosDb": {
      "ConnectionString": "Use user-secrets or environment variables to retrieve",
      "DatabaseName": "Use user-secrets or environment variables to retrieve",
      "ContainerName": "Use user-secrets or environment variables to retrieve"
    },
    "ComputerVision": {
      "Endpoint": "Use user-secrets or environment variables to retrieve",
      "Key": "Use user-secrets or environment variables to retrieve"
    }
  }
}
```

## 📦 Run the API
```bash
dotnet restore
dotnet build
dotnet run
```

## 📸 Sample Workflow
 - POST /api/image/upload – Uploads an image and analyzes it.
 - GET /api/image/search?query=cat – Searches metadata by tag or OCR text.

## 📂 Project Structure
```
├── Controllers
│   └── ImageController.cs
├── Services
│   ├── ImageStorageService.cs
│   ├── ImageAnalysisService.cs
│   └── ImageMetadataService.cs
├── Configuration
│   └── BlobStorageOptions.cs
│   └── ComputerVisionOptions.cs
│   └── CosmosDbOptions.cs
├── Models
│   └── ImageMetadata.cs
└── Program.cs
```

## 📄 License
This project is for personal or educational use only.

## 🙋‍♂️ About
This app was built to practice .NET 8, Azure development, and AI service integration. Front end with Angular coming soon.