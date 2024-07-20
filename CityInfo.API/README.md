# CityInfo REST Web API

The `CityInfo` is a RESTful Web API developed with the purpose of providing information about various cities.

## Key Technologies and Frameworks

- Platform: .NET Core.
- Web technology: ASP.NET Core.
- Language: C# (Version 12.0).
- Data format: JSON.
- Architecture: Model View Controller (MVC).
- Software Development Kit (SDK): .NET 8.0.

## Introduction

This API provides endpoints to retrieve city-related data in a structured format. The data is accessible through specific endpoints and can be consumed by client applications.

## Development Environment

The recommended integrated development environment (IDE) for this project is JetBrains Rider 2024.1.4, which is hosted on macOS Sonoma (x86_64).

JetBrains Rider provides a full-fledged development environment that is efficient and fluid, streamlining the entire development process, from writing the code, running tests to debugging, and deployment.

## Usage

The `CityInfo` API supports standard HTTP methods to create, read, update, and delete resources. The data returned by the API is in JSON format, which is easy to consume by any client application regardless of the platform it is running on.

Please refer to the API's endpoint documentation for a detailed view on how to interact with the `CityInfo` API.

## Controllers Overview

This project contains the following controllers, each handles different aspects of the CityInfo REST API:

1. **Cities Controller**: This controller is responsible for managing the cities. Common methods in this controller might be for listing all cities, retrieving information for a specific city, adding a new city, or deleting a city.

2. **Files Controller**: This controller is responsible for managing the file operations. Common methods in this controller might be for uploading a file, downloading a file, or deleting a file.

3. **Points Of Interest Controller**: This controller is responsible for managing Points of Interest (POI) within cities. This could involve retrieving POIs for a city, adding a new POI, updating, or deleting a POI.

Please refer to the actual implementation in the respective controllers for accurate detail about methods and their functionality.