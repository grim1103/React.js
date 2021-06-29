
 package net.cart.action;

import java.io.PrintWriter;


import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import net.action.Action;
import net.action.ActionForward;
import net.cart.db.CartBean;
import net.cart.db.CartDAO;

 public class CartListAction implements Action {
	 public ActionForward execute(HttpServletRequest request,HttpServletResponse response) throws Exception{
		 HttpSession session = request.getSession();
		 CartDAO cartdao = new CartDAO();
		 CartBean cartbean = new CartBean();
		 
		 String id = (String)session.getAttribute("id");
		 
		 if(cartdao.getListCart(id) == null) {
				PrintWriter out = response.getWriter();
				out.println("<script>");
				out.println("alert('There is no data please try again')");
				out.print("</script>");
				out.close();
				return null;
			}
		 
		 session.setAttribute("cartbean", cartdao.getListCart(id));
		 ActionForward forward= new ActionForward();
	   	 forward.setRedirect(false);
   		 forward.setPath("./cart/cart_list.jsp");
   		cartdao.conClose();
   		return forward;
	 }
 }
