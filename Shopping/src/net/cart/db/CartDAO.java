package net.cart.db;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

import javax.naming.Context;
import javax.naming.InitialContext;
import javax.sql.DataSource;

import net.product.db.ProductBean;

public class CartDAO {

	Connection conn = null;
	PreparedStatement pt = null;
	ResultSet re = null;

	public CartDAO() { // 디비 연결
		try {
			Context init = new InitialContext();
			DataSource ds = (DataSource) init.lookup("java:comp/env/jdbc/OracleDB");
			conn = ds.getConnection();
		} catch (Exception ex) {
			System.out.println("DB 연결 실패 : " + ex);
			return;
		}
	}// DB연결
	
	//카트에담기
		public boolean insertCart(CartBean cartbean) throws SQLException {

			String sql = "insert into cart values(sqe_cart_num.nextval,?,?,?,?,?,?,?,?)";
			
			try {
				pt = conn.prepareStatement(sql);

				pt.setString(1, cartbean.getCart_id());
				pt.setInt(2, cartbean.getCart_code());
				pt.setString(3, cartbean.getCart_image());
				pt.setString(4, cartbean.getCart_name());
				pt.setInt(5, cartbean.getCart_price());
				pt.setInt(6, cartbean.getCart_count());
				pt.setInt(7, cartbean.getCart_delprice());
				pt.setInt(8, cartbean.getCart_sum());
				pt.executeUpdate();

				return true;

			} catch (RuntimeException er) {
				er.printStackTrace();
			} finally {
				try {
					if (pt != null) {
						pt.close();
						pt = null;
					}
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
			return false;
		}
		
		//카트에서가져오기
		public List getListCart(String id) throws SQLException {
			String sql = "select * from cart where cart_id=?";
			List list = new ArrayList();

			try {
				pt = conn.prepareStatement(sql);
				pt.setString(1, id);
				
				re = pt.executeQuery();
			
				while (re.next()) {
					CartBean cartbean = new CartBean();
					cartbean.setCart_id(re.getString("cart_id"));
					cartbean.setCart_code(re.getInt("cart_code"));
					cartbean.setCart_image(re.getString("cart_image"));
					cartbean.setCart_name(re.getString("cart_name"));
					cartbean.setCart_price(re.getInt("cart_price"));
					cartbean.setCart_count(re.getInt("cart_count"));
					cartbean.setCart_delprice(re.getInt("cart_delprice"));
					cartbean.setCart_sum(re.getInt("cart_sum"));
					cartbean.setCart_num(re.getInt("cart_num"));

					list.add(cartbean);
				}

				return list;

			} catch (RuntimeException er) {
				er.printStackTrace();
			} finally {
				try {
					if (re != null) {
						re.close();
						re = null;
					}
					if (pt != null) {
						pt.close();
						pt = null;
					} // 닫아줌
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
			return null;
		}
		
		//

		public ProductBean detailCart(int code) throws SQLException{
			String sql = "select * from product where product_code = ?";
			try {
				pt = conn.prepareStatement(sql);
				pt.setInt(1, code);
				re = pt.executeQuery();
				ProductBean productbean = new ProductBean();
				
				if(!re.next()) {return null;}
				
				productbean.setProduct_code(re.getInt("product_code"));
				productbean.setProduct_category(re.getString("product_category"));
				productbean.setProduct_name(re.getString("product_name"));
				productbean.setProduct_count(re.getInt("product_count"));
				productbean.setProduct_image(re.getString("product_image"));
				productbean.setProduct_cost(re.getInt("product_cost"));
				productbean.setProduct_price(re.getInt("product_price"));
				productbean.setProduct_detail(re.getString("product_detail"));
				String date = String.valueOf(re.getTimestamp("product_date"));
				productbean.setProduct_date(date);
				
				
				return productbean;
			}catch(RuntimeException er) {
				er.printStackTrace();
			}finally {
				try {

					if(re!=null) { re.close(); re=null;}
					if(pt!=null) { pt.close(); pt=null;}
				}catch(Exception e) {
					e.printStackTrace();
				}
			}
			return null;
		}
		// 삭제
			public boolean deleteCart(int num) throws SQLException {
				String sql = "delete from cart where cart_num = ?";
				try {
					pt = conn.prepareStatement(sql);
					pt.setInt(1, num);
					pt.executeUpdate();
					
					return true;
					
				}catch(RuntimeException er) {
					er.printStackTrace();
				}finally {
					try {
						if(pt!=null) {pt.close(); pt = null;}
					}catch(Exception e) {
						e.printStackTrace();
					}
				}
				return false;
			}	
			//커넥트 닫기
			public void conClose() {
				try {
					conn.close();
				}catch(Exception e) {
					e.printStackTrace();
				}
			}
	}