This project was created as a learning exercise to gain familiarity with Azure Blob Storage. It serves as a practical example of integrating Azure Blob Storage with ASP.NET Core Web API, including handling file uploads, validating file types, and managing blob operations.

## Features

- Upload Image: Upload image files to Azure Blob Storage with content type validation.
- Get Images: Retrieve a list of URLs of all images stored in the Azure Blob Storage container.
- Delete Image: Delete a specific image from the Azure Blob Storage container.

## Technologies Used

- ASP.NET Core Web API
- Azure Blob Storage
- File Type Checker (File.TypeChecker package)

## Endpoints

### Get Images

**GET api/v1/azure**

Retrieve a list of URLs of all images stored in the Azure Blob Storage container.

**Response**

- 200 OK with a list of image URLs.

### Upload Image

**POST api/v1/azure/upload**

Upload an image file to Azure Blob Storage.

**Request**

- Form-data with key file containing the image file.

**Response**

- 200 OK if the upload is successful.
- 400 Bad Request if the file is not a validimage.

### Delete Image

**DELETE api/v1/azure/delete/{fileName}**

Delete a specific image from the Azure Blob Storage container.

**Response**

- 200 OK if the deletion is successful.
- 404 Not Found if the image does not exist.
