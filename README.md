# node-rgb-net

Simple Node JS wrapper for [RGB.NET](https://github.com/DarthAffe/RGB.NET) by [DarthAffe](https://github.com/DarthAffe) 

## Install
```
npm install node-rgb-net
```

## Usage
```js
var { getRgbDevices, setColor, DEVICE_TYPE } = require("./index.js")

getRgbDevices(DEVICE_TYPE.KEYBOARD, (err, data) => {
  console.log(data)
})

setColor({deviceType: DEVICE_TYPE.KEYBOARD, color: '#5500FF'}, (err, data) => {
  if (err) {
    console.log("Oops! Something happened")
  } else {
    console.log("LIGHT ON!")
  }
})

```

## Special Thanks to
[DarthAffe](https://github.com/DarthAffe) - Creator of [RGB.NET](https://github.com/DarthAffe/RGB.NET) 

## License
MIT © Riva Farabi
