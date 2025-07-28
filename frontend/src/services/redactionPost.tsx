import axios from "axios";

const apiUrl = axios.create({
    // baseURL: process.env.REACT_APP_API_BASE_URL,
    baseURL: "https://glorious-train-7v7w9jxjjgjfpq5j-5000.app.github.dev"
})

export default async function redactionPost(files: File[]) {
  const formData = new FormData();
  files.forEach((file) => formData.append("formData", file));

  console.log("API base URL:", process.env.REACT_APP_API_BASE_URL);

  try {
    const response = await apiUrl.post("/api/redaction/text/redact", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    console.log(response.data);
    return {
      success: true,
      message: response.data,
    };
  } catch (error: any) {
    console.error("Error uploading:", error);
    return {
      success: false,
      message: error?.response?.data || "An error occurred.",
    };
  }
}
