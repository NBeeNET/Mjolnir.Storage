<h1 align="center">
    <img src="/docs/images/logo.png" alt="NBeeNET" width="175"/>
    <br>
    NBeeNET - Mjolnir.Storage
    <br>
    <br>
    <a href="#" rel="nofollow"><img src="https://ci.appveyor.com/api/projects/status/8ypr7527dnao04yr/branch/develop?svg=true" alt="Build status" data-canonical-src="https://ci.appveyor.com/api/projects/status/8ypr7527dnao04yr/branch/Framework?svg=true" style="max-width:100%;"></a>
<a href="#" rel="nofollow"><img src="https://img.shields.io/github/issues-raw/JimBobSquarePants/imageprocessor.svg" alt="Issues open" style="max-width:100%;"></a>
<a href="#" rel="nofollow"><img src="https://img.shields.io/badge/Browse-Source-green.svg" alt="Source Browser" style="max-width:100%;"></a>
<a href="#" rel="nofollow"><img src="https://badges.gitter.im/Join%20Chat.svg" style="max-width:100%;"></a>
</h1>

**Mjolnir.Storage** 是一个基于 .Net Core + Restful API 开发的对象存储代理作业服务。

**Mjolnir.Storage 能干什么？**

- Mjolnir.Storage 提供标准的 Restful API 文件上传接口，可以直接与各类终端对接使用。
- 上传到 Mjolnir.Storage 中的文件支持多种作业处理。
- 上传的源文件及作业处理过的文件可以自动的存储到本地、AWS S3、Azure Blob等对象存储中。

<h1 align="center">
    <img src="/docs/images/Mjolnir.Storage.png" alt="Mjolnir.Storage" width="600"/>
    <br>
</h1>

**Library 说明**

| Library | 说明 |
| :--- | :--- |
| **NBeeNET.Mjolnir.Storage.Core** | 实现公共及核心业务 |
| **NBeeNET.Mjolnir.Storage.Image** | 实现图片上传及作业处理 |
| **NBeeNET.Mjolnir.Storage.Local** | 实现本地存储 |
| **NBeeNET.Mjolnir.Storage.AzureBlob** | 实现Azure Blob存储 |
| **NBeeNET.Mjolnir.Storage.AWSS3** | 实现AWS S3存储 |

<!-- ## 支持存储

- 图片：生成缩略图、格式转换、加水印、AI物体识别
- 视频：生成封面图、视频转码、加水印
- 音频：音频转码、AI文字识别

## 123 -->

