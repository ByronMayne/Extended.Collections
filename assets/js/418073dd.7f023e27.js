"use strict";(self.webpackChunkextended_collections=self.webpackChunkextended_collections||[]).push([[21],{3905:(e,n,t)=>{t.d(n,{Zo:()=>c,kt:()=>g});var r=t(7294);function a(e,n,t){return n in e?Object.defineProperty(e,n,{value:t,enumerable:!0,configurable:!0,writable:!0}):e[n]=t,e}function i(e,n){var t=Object.keys(e);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(e);n&&(r=r.filter((function(n){return Object.getOwnPropertyDescriptor(e,n).enumerable}))),t.push.apply(t,r)}return t}function o(e){for(var n=1;n<arguments.length;n++){var t=null!=arguments[n]?arguments[n]:{};n%2?i(Object(t),!0).forEach((function(n){a(e,n,t[n])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(t)):i(Object(t)).forEach((function(n){Object.defineProperty(e,n,Object.getOwnPropertyDescriptor(t,n))}))}return e}function l(e,n){if(null==e)return{};var t,r,a=function(e,n){if(null==e)return{};var t,r,a={},i=Object.keys(e);for(r=0;r<i.length;r++)t=i[r],n.indexOf(t)>=0||(a[t]=e[t]);return a}(e,n);if(Object.getOwnPropertySymbols){var i=Object.getOwnPropertySymbols(e);for(r=0;r<i.length;r++)t=i[r],n.indexOf(t)>=0||Object.prototype.propertyIsEnumerable.call(e,t)&&(a[t]=e[t])}return a}var s=r.createContext({}),f=function(e){var n=r.useContext(s),t=n;return e&&(t="function"==typeof e?e(n):o(o({},n),e)),t},c=function(e){var n=f(e.components);return r.createElement(s.Provider,{value:n},e.children)},d="mdxType",u={inlineCode:"code",wrapper:function(e){var n=e.children;return r.createElement(r.Fragment,{},n)}},m=r.forwardRef((function(e,n){var t=e.components,a=e.mdxType,i=e.originalType,s=e.parentName,c=l(e,["components","mdxType","originalType","parentName"]),d=f(t),m=a,g=d["".concat(s,".").concat(m)]||d[m]||u[m]||i;return t?r.createElement(g,o(o({ref:n},c),{},{components:t})):r.createElement(g,o({ref:n},c))}));function g(e,n){var t=arguments,a=n&&n.mdxType;if("string"==typeof e||a){var i=t.length,o=new Array(i);o[0]=m;var l={};for(var s in n)hasOwnProperty.call(n,s)&&(l[s]=n[s]);l.originalType=e,l[d]="string"==typeof e?e:a,o[1]=l;for(var f=2;f<i;f++)o[f]=t[f];return r.createElement.apply(null,o)}return r.createElement.apply(null,t)}m.displayName="MDXCreateElement"},5381:(e,n,t)=>{t.r(n),t.d(n,{assets:()=>s,contentTitle:()=>o,default:()=>u,frontMatter:()=>i,metadata:()=>l,toc:()=>f});var r=t(7462),a=(t(7294),t(3905));const i={},o="Ring Buffer",l={unversionedId:"generic/ring_buffer",id:"generic/ring_buffer",title:"Ring Buffer",description:"A ring buffer is a data structure that efficiently manages a fixed-size, cyclically-referenced buffer, allowing for constant-time insertions and removals while overwriting the oldest data when full.",source:"@site/../documentation/generic/ring_buffer.md",sourceDirName:"generic",slug:"/generic/ring_buffer",permalink:"/Extended.Collections/generic/ring_buffer",draft:!1,editUrl:"https://github.com/ByronMayne/Extended.Collections/edit/main/documentation/generic/ring_buffer.md",tags:[],version:"current",lastUpdatedAt:1698018654,formattedLastUpdatedAt:"Oct 22, 2023",frontMatter:{},sidebar:"defaultSidebar",previous:{title:"Deque",permalink:"/Extended.Collections/generic/deque"}},s={},f=[{value:"Advantages",id:"advantages",level:2},{value:"Disadvantages",id:"disadvantages",level:2}],c={toc:f},d="wrapper";function u(e){let{components:n,...t}=e;return(0,a.kt)(d,(0,r.Z)({},c,t,{components:n,mdxType:"MDXLayout"}),(0,a.kt)("h1",{id:"ring-buffer"},"Ring Buffer"),(0,a.kt)("p",null,"A ring buffer is a data structure that efficiently manages a fixed-size, cyclically-referenced buffer, allowing for constant-time insertions and removals while overwriting the oldest data when full."),(0,a.kt)("h2",{id:"advantages"},"Advantages"),(0,a.kt)("ul",null,(0,a.kt)("li",{parentName:"ul"},(0,a.kt)("strong",{parentName:"li"},"Constant Time Complexity for Insertions and Deletions"),": Ring buffers offer O(1) time complexity for both inserting elements (enqueue) and removing elements (dequeue) as long as the buffer is not full or empty. This makes them highly efficient for real-time applications where performance matters."),(0,a.kt)("li",{parentName:"ul"},(0,a.kt)("strong",{parentName:"li"},"Fixed and Predictable Memory Usage"),": Ring buffers have a fixed capacity, which means that memory usage is predictable and doesn't grow beyond the specified limit. This characteristic is crucial in embedded systems and other resource-constrained environments."),(0,a.kt)("li",{parentName:"ul"},(0,a.kt)("strong",{parentName:"li"},"Efficient for Streaming and Circular Data"),": Ring buffers are ideal for managing streaming data, such as audio, video, or sensor data, as they provide a continuous loop for storing and processing the most recent data points."),(0,a.kt)("li",{parentName:"ul"},(0,a.kt)("strong",{parentName:"li"},"No Need for Dynamic Memory Allocation"),": Unlike dynamic data structures like linked lists or arrays with variable sizes, ring buffers do not require dynamic memory allocation or deallocation. This eliminates potential memory fragmentation issues and improves memory management efficiency.")),(0,a.kt)("h2",{id:"disadvantages"},"Disadvantages"),(0,a.kt)("ul",null,(0,a.kt)("li",{parentName:"ul"},(0,a.kt)("strong",{parentName:"li"},"Fixed Capacity Limitation"),": The fixed size of a ring buffer can be a disadvantage if the number of elements you need to store exceeds the buffer's capacity. In such cases, you would need to implement additional logic for handling overflow conditions."),(0,a.kt)("li",{parentName:"ul"},(0,a.kt)("strong",{parentName:"li"},"Inefficient for Non-Circular Data"),": Ring buffers are designed for circular data usage, so they are not suitable for scenarios where you need to retain all historical data without overwriting any of it."),(0,a.kt)("li",{parentName:"ul"},(0,a.kt)("strong",{parentName:"li"},"Insertions May Overwrite Data"),": When the buffer is full and new elements are inserted, they overwrite the oldest elements. This behavior may not be suitable for all applications, especially if you need to preserve a complete history of data.")),(0,a.kt)("h1",{id:"code"},"Code"),(0,a.kt)("pre",null,(0,a.kt)("code",{parentName:"pre",className:"language-csharp",metastring:"file=../../src/Extended.Collections.Playground/Generic/RingBufferSandbox.cs#L2-",file:"../../src/Extended.Collections.Playground/Generic/RingBufferSandbox.cs#L2-"},'using Extended.Collections.Generic;\n\npublic class RingBufferSandbox : Sandbox\n{\n    private readonly RingBuffer<string> m_buffer = new (3);\n\n    protected override void Run()\n    {\n        m_buffer.Add("A");\n        m_buffer.Add("B");\n        m_buffer.Add("C");\n        Logger.Information("1. {Buffer}", m_buffer); // 1. [ "A", "B", "C" ]\n\n        m_buffer.Add("D");\n        Logger.Information("2. {Buffer}", m_buffer); // 2. [ "B", "C", "D" ]\n\n        m_buffer.Remove("C");\n        Logger.Information("3. {Buffer}", m_buffer); // 3. [ "B", "D" ]\n\n        m_buffer.Add("E");\n        Logger.Information("4. {Buffer}", m_buffer); // 4. [ "B", "D", "E" ]\n\n        m_buffer.Clear();\n        Logger.Information("5. {Buffer}", m_buffer); // [ ]\n    }\n}\n')))}u.isMDXComponent=!0}}]);