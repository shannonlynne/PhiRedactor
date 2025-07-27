import axios from "axios";

const apiUrl = axios.create({
  baseURL: process.env.REACT_APP_API_BASE_URL,
});

export default async function redactionPost(files: File[]) {
  const formData = new FormData();
  files.forEach((file) => formData.append("formData", file));

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
