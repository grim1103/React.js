<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
	
	
	
<!DOCTYPE html>
<html lang="ko">
<head>
<meta charset="UTF-8">

<style>

h1 {
	padding: 50px 0;
	font-weight: 400;
	text-align: center;
}

p {
	margin: 0 0 20px;
	line-height: 1.5;
}

.main {
	min-width: 320px;
	max-width: 850px;
	padding: 50px;
	margin: 0 auto;
	background: #ffffff;
}

section {
	display: none;
	padding: 20px 0 0;
	border-top: 1px solid #ddd;
}

/*라디오버튼 숨김*/
input {
	display: none;
}

label {
	display: inline-block;
	margin: 0 0 -1px;
	padding: 15px 25px;
	font-weight: 600;
	text-align: center;
	color: #bbb;
	border: 1px solid transparent;
}

label:hover {
	color: #2e9cdf;
	cursor: pointer;
}

/*input 클릭시, label 스타일*/
input:checked+label {
	color: #555;
	border: 1px solid #ddd;
	border-top: 2px solid #2e9cdf;
	border-bottom: 1px solid #ffffff;
}

#tab1:checked ~ #content1, #tab2:checked ~ #content2 {
	display: block;
}
</style>

<title>Document</title>
<link rel="stylesheet" href="../css/style.css" />
</head>
<body>
<center>

<div id="login_div"><jsp:include page="/header.jsp"></jsp:include></div>

<jsp:include page="basket.jsp"></jsp:include>

</center>



	
</body>

</html>

