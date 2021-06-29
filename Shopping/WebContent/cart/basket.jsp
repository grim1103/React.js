<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
<%@page import="java.util.List" %>
<%@page import="net.cart.db.CartBean"%>
<!DOCTYPE html>
<html lang="ko">
<head>
<meta charset="UTF-8">
<style>
/* body {
	color: #555;
	background: #eeeeee;
	margin: 0;
	padding: 0;
	box-sizing: border-box;
} */

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

</style>

<title>Document</title>
</head>
<body>
<%
	List<CartBean> beans = (List<CartBean>)session.getAttribute("cartbean");
%>


	<div class="main">
<br><br><br>

		<div>
			<div>
				<div>
					<h1>
						<span><b>SHOPPING CART</b></span>
					</h1>
				</div>
				
				<div class="description">
					<div class="member ">
								 <table border="1" summary="" class="cart">
									
									
									<thead>
										<tr>
											<th scope="col"><h5>상품코드</h5></th>
											<th scope="col"><h5>이미지</h5></th>
											<th scope="col"><h5>상품이름</h5></th>
											<th scope="col"><h5>판매가</h5></th>
											<th scope="col"><h5>수량</h5></th>
											<th scope="col"><h5>합계</h5></th>
											<th scope="col"><h5>선택</h5></th>
										</tr>
										<% for(CartBean cartbean : beans){%>
										<tr>	
											<td><%= cartbean.getCart_code() %></td>
											<td><%= cartbean.getCart_image() %></td>
											<td><%= cartbean.getCart_name() %></td>
											<td><%= cartbean.getCart_price() %></td>
											<td><%= cartbean.getCart_count() %></td>
											<td><%= cartbean.getCart_sum() %></td>
											<td></td>
											
										</tr>
										<%} %>
									</thead>
								</table>
								<br> <br>
								<th>

									<table border="1">
									<thead>
									<th>총 상품금액</th>
									<th>배송비</th>
									<th>결제예정 금액</th>
									</thead> 
									<% int sum=0; %>
									<%for(CartBean cartbean : beans){
										
										sum+=cartbean.getCart_sum();
									}
									%>
									
									<tr>
									<td><%=sum %></td>
									<td><%= 2500 %></td>
									<td><%= sum+2500 %></td>
									</tr>
									</table>
								</th>
							</section>

					</div>



					<!-- 이용안내 -->
					<div>
					<br><br><br>
						<div class="inner">
							<h3>*장바구니 이용안내*</h3>
							<ol>
								<li>배송비는  2500원 입니다.</li>
								<li>[쇼핑계속하기] 버튼을 누르시면 쇼핑을 계속 하실 수 있습니다.</li>
								<li>[전체상품주문하기] 버튼을 누르시면 전체상품을 주문 하실 수 있습니다.</li>
								<li>[선택상품주문하기] 버튼을 누르시면 선택상품을 주문 하실 수 있습니다.</li>
							</ol>
							
							<form action="cart_list.jsp" method="post">
								<input type="submit" value="결제하기" />
							</form>
						</div>
						<br> <br>
						<!-- 주문 버튼 -->
						<div align="center"
							class="">
							<a href="#" onclick="Basket.orderAll(this)">
							<img src="image/1.jpg" alt="전체상품주문" /></a>
						    <a href="#"
								onclick="Basket.orderSelectBasket(this)">
							<img src="image/2.jpg" alt="선택상품주문" /></a><span class="gRight">
							<a href="#"><img src="image/3.jpg" alt="쇼핑계속하기" /></a>
							</span>
						</div>
					</div>
				</div>
			</div>
		</div>
		<br><br><br><br><br><br><a href="#header"><img src="image/bottom1.png" width="70px"
				height="70px" /></a>
</body>
</html>