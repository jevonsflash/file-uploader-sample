<template>
  <div id="app">
    <!-- <img alt="Vue logo" src="./assets/logo.png" />
    <HelloWorld msg="Welcome to Your Vue.js App" /> -->

    <el-upload
      ref="upload"
      :limit="10"
      multiple
      :before-upload="beforeUpload"
      :on-success="handleSuccess"
      :on-remove="handleRemove"
      :on-error="handleError"
      action="/"
      :http-request="upload"
    >
      <el-button ref="uploadButton">上传</el-button>
      <span slot="file" slot-scope="{ file }">
        <div class="filelist-item">
          <el-row>
            <el-col :span="6" class="file-icon-frame">
              <i class="el-icon-document file-icon"></i>
            </el-col>
            <el-col :span="18">
              <el-row>
                <el-col :span="20">
                  <label class="file-title">
                    {{ file.name }}
                  </label>
                </el-col>
                <el-col :span="4" style="text-align: right">
                  <el-button
                    type="danger"
                    icon="el-icon-minus"
                    size="mini"
                    circle
                    @click="handleRemove(file)"
                  ></el-button>
                </el-col>
                <el-col :span="24">
                  <label class="file-size">
                    {{ FriendlyFileSize(file.size) }}
                  </label>
                </el-col>
                <el-col :span="24">
                  <el-progress
                    :text-inside="true"
                    :stroke-width="26"
                    :percentage="parseInt(file.percentage, 10)"
                    :status="
                      parseInt(file.percentage, 10) == 100 ? 'success' : ''
                    "
                  >
                  </el-progress
                ></el-col>
              </el-row>
            </el-col>
          </el-row>
        </div>
      </span>
    </el-upload>
  </div>
</template>

<script lang='ts'>
import Enumerable from "linq";
import HelloWorld from "./components/HelloWorld.vue";
import { getCancelToken, request } from "./ajaxRequire";

export class CreateFileDto {
  id: string;
  fileContainerName: string;
  parentId: string;
  ownerUserId: number;
  fileName: string;
  mimeType: string;
  fileType: number;
  file: any;
}

export default {
  name: "App",
  // components: {HelloWorld},
  data: () => {
    return {
      showHistory: false,
      loading: false,
      fileSizeLimit: -1,
      uploadUrl: "http://localhost:14149/file/upload",
    };
  },

  methods: {
    successMessage(value = "执行成功") {
      this.$notify({
        title: "成功",
        message: value,
        type: "success",
      });
    },

    errorMessage(value = "执行错误") {
      this.$notify.error({
        title: "错误",
        message: value,
      });
    },

    handleSuccess(response, file, fileList) {
      this.successMessage("上传成功");
      this.loading = false;
    },

    handleError(e, file, fileList) {
      this.errorMessage(e);
      this.loading = false;
    },

    handleRemove(file, fileList) {
      if (file.raw.cancelToken) {
        file.raw.cancelToken.cancel();
      }
    },

    async upload(option) {
      var model = new CreateFileDto();
      var file = option.file;
      model.fileName = file.name;
      model.fileType = 2;
      model.mimeType = file.type;
      model.ownerUserId = 1;
      model.fileContainerName = "Container1";
      model.file = file;
      var fd = new FormData();

      Enumerable.from(model).forEach((c) => {
        fd.append(c.key, c.value);
      });

      var token = file.cancelToken;
      await request(
        this.uploadUrl,
        "post",
        fd,
        (e) => {
          if (e.total > 0) {
            e.percent = (e.loaded / e.total) * 100;
          }
          option.onProgress(e);
        },
        token
      );
    },

    beforeUpload(file) {
      var token = getCancelToken();
      file.cancelToken = token;
      let isLt2M = true;
      if (this.fileSizeLimit < 0) {
        return true;
      }
      isLt2M = file.size / 1024 / 1024 < this.fileSizeLimit;
      if (!isLt2M) {
        this.loading = false;
        this.errorMessage(`"上传文件大小不能超过 ${this.fileSizeLimit}}MB!"`);
      }
      return isLt2M;
    },

    FriendlyFileSize(bytes) {
      bytes = parseFloat(bytes);
      if (bytes === 0) return "0B";
      let k = 1024,
        sizes = ["B", "KB", "MB", "GB", "TB"],
        i = Math.floor(Math.log(bytes) / Math.log(k));
      return (bytes / Math.pow(k, i)).toPrecision(3) + sizes[i];
    },
  },
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: left;
  color: #2c3e50;
  margin-top: 60px;
}
.el-upload-list__item .el-progress {
  position: inherit;
}
.filelist-item {
  padding: 20px 20px 20px 0;
  box-shadow: rgba(0, 0, 0, 0.12) 0px 2px 4px, rgba(0, 0, 0, 0.04) 0px 0px 6px;
  border-radius: 10px;
}
.file-icon-frame {
  text-align: center;
  margin-top: 10px;
}
.file-icon-frame .file-icon {
  font-size: 70px;
}
.file-size {
  font-size: small;
  color: gray;
}
.file-title {
  font-size: large;
  font-weight: bolder;
}
</style>
